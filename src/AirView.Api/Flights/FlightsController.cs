using System;
using System.Threading.Tasks;
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

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _queryableRepository
                .QueryAll()
                .ToListAsync());

        [HttpGet("{id}", Name = "get-flight")]
        public async Task<IActionResult> GetById(Guid id) =>
            (await _queryableRepository.TryFindAsync(id))
            .Map<FlightProjection, IActionResult>(Ok)
            .Reduce(NotFound);

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterFlightDto dto)
        {
            if (dto == null) return BadRequest();

            return (await _commandSender.SendAsync(_mapper.Map<RegisterFlightCommand>(dto)))
                .Map(id => Accepted(Link(id)) as IActionResult)
                .ReduceOrThrow();
        }

        [HttpPut("{id}/schedule")]
        public async Task<IActionResult> Schedule(Guid id, [FromBody] ScheduleFlightDto dto)
        {
            if (dto == null) return BadRequest();

            var flightId = id;
            var command = _mapper.Map<ScheduleFlightCommand>(
                dto, opt => opt.Items[nameof(ScheduleFlightCommand.Id)] = flightId);

            return (await _commandSender.SendAsync(command))
                .Map(() => Accepted(Link(id)) as IActionResult)
                .Reduce<EntityNotFoundCommandException<ScheduleFlightCommand>>(_ => NotFound())
                .ReduceOrThrow();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unregister(Guid id) =>
            (await _commandSender.SendAsync(new UnregisterFlightCommand(id)))
            .Map(() => Accepted(Link(id)) as IActionResult)
            .Reduce<EntityNotFoundCommandException<UnregisterFlightCommand>>(_ => NotFound())
            .ReduceOrThrow();

        private string Link(Guid id) =>
            Url.Link("get-flight", new {id = id.ToString()});
    }
}