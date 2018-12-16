using System;
using System.Linq;
using System.Net;
using AirView.Application;
using AirView.Application.Core;
using AirView.Domain;
using AirView.Persistence;
using AirView.Persistence.Core;
using AirView.Persistence.Core.EntityFramework;
using AirView.Persistence.Core.EntityFramework.EventSourcing;
using AirView.Shared.Railways;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

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

            app.UseExceptionHandler(appBuilder =>
                appBuilder.Run(async context =>
                {
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionFeature.Error;

                    var result = env.IsDevelopment()
                        ? new
                        {
                            exception.Message,
                            StackTrace = exception.StackTrace.Split("\r\n").Select(line => line.Trim()).ToArray()
                        }
                        : new {Message = "An unexpected fault happened. Please try again later."} as object;

                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(result));
                }));

            app.UseHttpsRedirection();
            app.UseMvc();

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<WriteDbContext>().Database.EnsureCreated();
                //serviceScope.ServiceProvider.GetRequiredService<ReadDbContext>().Database.EnsureCreated();
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
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<WriteDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork, EntityFrameworkUnitOfWork<WriteDbContext>>();
            services.AddTransient<
                IQueryableRepository<Guid, Flight>,
                EntityFrameworkRepository<Guid, Flight, ReadDbContext>>();
            services.AddTransient<
                IWritableRepository<Guid, Flight>,
                EntityFrameworkEventSourcedRepository<Guid, Flight, WriteDbContext>>();
            // == Application ==
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

            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
    }
}