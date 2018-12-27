using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using AirView.Api.Core;
using AirView.Api.Core.Internal;
using AirView.Application;
using AirView.Application.Core;
using AirView.Domain;
using AirView.Domain.Core;
using AirView.Persistence;
using AirView.Persistence.Core;
using AirView.Persistence.Core.EntityFramework;
using AirView.Persistence.Core.EntityFramework.EventSourcing;
using AirView.Shared.Railways;
using AutoMapper;
using GlobalExceptionHandler.ContentNegotiation.Mvc;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AirView.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (!env.IsDevelopment())
                app.UseHsts();

            app.UseGlobalExceptionHandler(options =>
            {
                options.ContentType = "application/problem+json";
                options.Map<Exception>().ToStatusCode(HttpStatusCode.InternalServerError).WithBody(exception =>
                {
                    object problemDetails = new ProblemDetails
                    {
                        Type = "about:blank",
                        Title = "An unexpected error happened.",
                        Status = (int?) HttpStatusCode.InternalServerError,
                        Detail = env.IsDevelopment()
                            ? exception.Message
                            : "We are unable to give you more detail about this error. Please try again and contact support if the problem persists.",
                        Instance = "about:blank"
                    };

                    if (env.IsDevelopment())
                        problemDetails = problemDetails.Assign(new
                        {
                            StackTrace = exception.Demystify().StackTrace
                                .Split("\r\n")
                                .Select(line => line.Trim())
                                .ToArray()
                        });

                    return problemDetails;
                });
            });

            app.UseHttpsRedirection();
            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<ReadDbContext>().Database.EnsureCreated();
                serviceScope.ServiceProvider.GetRequiredService<WriteDbContext>().Database.EnsureCreated();
            }
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // == Persistence ==
            services.AddDbContext<ReadDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Read")));
            services.AddDbContext<WriteDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Write")));
            services.AddScoped<IReadUnitOfWork>(provider =>
                new EntityFrameworkUnitOfWork(provider.GetRequiredService<ReadDbContext>()));
            services.AddScoped<IWriteUnitOfWork>(provider =>
                new EntityFrameworkUnitOfWork(provider.GetRequiredService<WriteDbContext>()));
            services.AddScoped<IQueryableRepository<FlightProjection>>(provider =>
                new EntityFrameworkRepository<FlightProjection>(provider.GetRequiredService<ReadDbContext>()));
            services.AddScoped<IWritableRepository<FlightProjection>>(provider =>
                new EntityFrameworkRepository<FlightProjection>(provider.GetRequiredService<ReadDbContext>()));
            services.AddScoped<IWritableRepository<Flight>>(provider =>
                new EntityFrameworkEventSourcedRepository<Flight>(
                    provider.GetRequiredService<WriteDbContext>(), provider.GetRequiredService<IEventPublisher>()));
            // == Application ==
            // TODO(maximegelinas): Scan assemblies to register every command handlers and every event handlers at once.
            services.AddTransient<
                ICommandHandler<RegisterFlightCommand, Result<CommandException<RegisterFlightCommand>, Guid>>,
                RegisterFlightCommandHandler>();
            services.AddTransient<
                ICommandHandler<ScheduleFlightCommand, Result<CommandException<ScheduleFlightCommand>>>,
                ScheduleFlightCommandHandler>();
            services.AddTransient<
                ICommandHandler<UnregisterFlightCommand, Result<CommandException<UnregisterFlightCommand>>>,
                UnregisterFlightCommandHandler>();
            services.AddScoped<ICommandSender, InMemoryBus>(provider =>
                new InMemoryBusBuilder()
                    .AddCommandHandler(provider.GetRequiredService<ICommandHandler<
                        RegisterFlightCommand, Result<CommandException<RegisterFlightCommand>, Guid>>>())
                    .AddCommandHandler(provider.GetRequiredService<ICommandHandler<
                        ScheduleFlightCommand, Result<CommandException<ScheduleFlightCommand>>>>())
                    .AddCommandHandler(provider.GetRequiredService<ICommandHandler<
                        UnregisterFlightCommand, Result<CommandException<UnregisterFlightCommand>>>>())
                    .Build());
            services.AddTransient<IEventHandler<IDomainEvent<Flight, FlightRegistratedEvent>>, FlightProjector>();
            services.AddTransient<IEventHandler<IDomainEvent<Flight, FlightScheduledEvent>>, FlightProjector>();
            services.AddTransient<IEventHandler<IDomainEvent<Flight, AggregateRemovedEvent>>, FlightProjector>();
            services.AddScoped<IEventPublisher, InMemoryBus>(provider =>
                new InMemoryBusBuilder()
                    .AddEventHandler(provider.GetRequiredService<IEventHandler<
                        IDomainEvent<Flight, FlightRegistratedEvent>>>())
                    .AddEventHandler(provider.GetRequiredService<IEventHandler<
                        IDomainEvent<Flight, FlightScheduledEvent>>>())
                    .AddEventHandler(provider.GetRequiredService<IEventHandler<
                        IDomainEvent<Flight, AggregateRemovedEvent>>>())
                    .Build());
            services.Configure<ApiBehaviorOptions>(options =>
                options.InvalidModelStateResponseFactory = context =>
                {
                    context.HttpContext.Response.ContentType = "application/problem+json";

                    return context.GetPathErrors().TryFirst()
                        .Map(modelError => new BadRequestObjectResult(new ProblemDetails
                        {
                            Type = "about:blank",
                            Title = "The given request URL is not valid.",
                            Status = (int?) HttpStatusCode.BadRequest,
                            Detail = modelError.ErrorMessage,
                            Instance = "about:blank"
                        }) as IActionResult)
                        .Reduce(() => context.GetUnsupportedContentTypeError()
                            .Map(modelError => new ObjectResult(new ProblemDetails
                            {
                                Type = "about:blank",
                                Title = "The given content type is not supported.",
                                Status = (int?) HttpStatusCode.UnsupportedMediaType,
                                Detail = modelError.Exception.Message,
                                Instance = "about:blank"
                            }) {StatusCode = (int?) HttpStatusCode.UnsupportedMediaType} as IActionResult)
                            .Reduce(() => context.GetBodyErrors().TryFirst()
                                .Map(modelError => new BadRequestObjectResult(new ProblemDetails
                                {
                                    Type = "about:blank",
                                    Title = "The given request body is not valid.",
                                    Status = (int?) HttpStatusCode.BadRequest,
                                    Detail = modelError.ErrorMessage,
                                    Instance = "about:blank"
                                }) as IActionResult)
                                .Reduce(() => new UnprocessableEntityObjectResult(
                                    new ProblemDetails
                                    {
                                        Type = "about:blank",
                                        Title = "One or more validation errors occurred.",
                                        Status = (int?) HttpStatusCode.UnprocessableEntity,
                                        Detail = "See errors for details.",
                                        Instance = "about:blank"
                                    }.Assign(new
                                    {
                                        Errors = new SerializableError(context.ModelState)
                                    })))));
                });
        }
    }
}