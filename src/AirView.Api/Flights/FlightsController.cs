using System;
using System.Threading.Tasks;
using AirView.Api.Core;
using AirView.Application;
using AirView.Application.Core;
using AirView.Application.Core.Exceptions;
using AirView.Domain;
using AirView.Persistence.Core;
using AirView.Shared.Railways;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AirView.Api.Flights
{
    [Route("api/flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly ICommandSender _commandSender;
        private readonly IMapper _mapper;
        private readonly IQueryableRepository<FlightProjection> _queryableRepository;

        public FlightsController(ICommandSender commandSender, IMapper mapper,
            IQueryableRepository<FlightProjection> queryableRepository)
        {
            _commandSender = commandSender;
            _mapper = mapper;
            _queryableRepository = queryableRepository;
        }

        [HttpGet("{id}", Name = "find")]
        public async Task<IActionResult> Find(Guid id) =>
            (await _queryableRepository.TryFindAsync(id))
            .Do(AddLinks)
            .Map<FlightProjection, IActionResult>(Ok)
            .Reduce(NotFound);

        [HttpGet(Name = "query")]
        public async Task<IActionResult> Query(int limit = 50, int offset = 0)
        {
            var query = _queryableRepository.Query().Paginate(limit, offset);
            var dto = await query.ToCollectionDtoAsync(await _queryableRepository.Query().CountAsync());
            AddLinks(dto, query);
            return Ok(dto);
        }

        [HttpPost(Name = "register")]
        public async Task<IActionResult> Register([FromBody] RegisterFlightDto dto) =>
            (await _commandSender.SendAsync(_mapper.Map<RegisterFlightCommand>(dto)))
            .Map(id => Accepted(Url.RouteUrl("find", new {id})) as IActionResult)
            .ReduceOrThrow();

        [HttpPut("{id}/schedule", Name = "schedule")]
        public async Task<IActionResult> Schedule(Guid id, [FromBody] ScheduleFlightDto dto)
        {
            var flightId = id;
            var command = _mapper.Map<ScheduleFlightCommand>(
                dto, opt => opt.Items[nameof(ScheduleFlightCommand.Id)] = flightId);

            return (await _commandSender.SendAsync(command))
                .Map(() => Accepted(Url.RouteUrl("find", new {id})) as IActionResult)
                .Reduce<EntityNotFoundCommandException<ScheduleFlightCommand>>(_ => NotFound())
                .ReduceOrThrow();
        }

        [HttpDelete("{id}", Name = "unregister")]
        public async Task<IActionResult> Unregister(Guid id) =>
            (await _commandSender.SendAsync(new UnregisterFlightCommand(id)))
            .Map(() => Accepted(Url.RouteUrl("find", new {id})) as IActionResult)
            .Reduce<EntityNotFoundCommandException<UnregisterFlightCommand>>(_ => NotFound())
            .ReduceOrThrow();

        private void AddLinks(FlightProjection dto)
        {
            dto.AddLink("self", new LinkDto(Url.RouteUrl("find", new {dto.Id})));
            dto.AddLink("schedule", new LinkDto(Url.RouteUrl("schedule", new {dto.Id})));
            dto.AddLink("unregister", new LinkDto(Url.RouteUrl("unregister", new { dto.Id })));
        }

        private void AddLinks(CollectionDto<FlightProjection> dto, IPagedQueryable<FlightProjection> query)
        {
            dto.AddLink("self", new LinkDto(RouteUrl("query", query)));

            if (query.HasPreviousPage())
            {
                dto.AddLink("first", new LinkDto(RouteUrl("query", query.FirstPage())));
                dto.AddLink("prev", new LinkDto(RouteUrl("query", query.PreviousPage())));
            }

            if (query.HasNextPage(dto.TotalCount))
            {
                dto.AddLink("next", new LinkDto(RouteUrl("query", query.NextPage(dto.TotalCount))));
                dto.AddLink("last", new LinkDto(RouteUrl("query", query.LastPage(dto.TotalCount))));
            }

            dto.AddLink("find", new LinkDto($"{Url.RouteUrl("query")}/{{id}}") { Templated = true });
            dto.AddLink("register", new LinkDto(Url.RouteUrl("register")));

            foreach (var value in dto.Values) AddLinks(value);
        }

        private string RouteUrl<T>(string routeName, IPagedQueryable<T> queryable) =>
            Url.RouteUrl(routeName, new {limit = queryable.Limit, offset = queryable.Offset});
    }
}