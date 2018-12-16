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

namespace AirView.Api.Flights
{
    [Route("api/flights")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly ICommandSender _commandSender;
        private readonly IMapper _mapper;
        private readonly IQueryableRepository<Guid, Flight> _queryableRepository;
        private readonly IWritableRepository<Guid, Flight> _writableRepository;

        public FlightsController(ICommandSender commandSender, IMapper mapper,
            IQueryableRepository<Guid, Flight> queryableRepository,
            IWritableRepository<Guid, Flight> writableRepository)
        {
            _commandSender = commandSender;
            _mapper = mapper;
            _queryableRepository = queryableRepository;
            _writableRepository = writableRepository;
        }

        [HttpGet]
        public IActionResult GetAll() =>
            Ok(new[]
            {
                new FlightDto
                {
                    Id = Guid.Parse("585e50f1-82c0-49fd-9d8c-d6ca57e64572")
                },
                new FlightDto
                {
                    Id = Guid.Parse("685e50f1-82c0-49fd-9d8c-d6ca57e64572")
                },
                new FlightDto
                {
                    Id = Guid.Parse("785e50f1-82c0-49fd-9d8c-d6ca57e64572")
                }
            });

        //[HttpGet]
        //public async Task<IActionResult> GetAll() =>
        //    Ok(await _queryableRepository
        //        .QueryAll()
        //        .ProjectTo<FlightDto>(_mapper.ConfigurationProvider)
        //        .ToListAsync());

        [HttpGet("{id}", Name = "get-flight")]
        public async Task<IActionResult> GetById(Guid id) =>
            (await _writableRepository.TryFindAsync(id))
            .Map(_mapper.Map<FlightDto>)
            .Map<FlightDto, IActionResult>(Ok)
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

            var command =
                _mapper.Map<ScheduleFlightCommand>(dto,
                    opt => opt.Items[nameof(ScheduleFlightCommand.Id)] = id);

            return (await _commandSender.SendAsync(command))
                .Map(() => NoContent() as IActionResult)
                .Reduce<EntityNotFoundCommandException<ScheduleFlightCommand>>(_ => NotFound())
                .ReduceOrThrow();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Unregister(Guid id) =>
            (await _commandSender.SendAsync(new UnregisterFlightCommand(id)))
            .Map(() => NoContent() as IActionResult)
            .Reduce<EntityNotFoundCommandException<UnregisterFlightCommand>>(_ => NotFound())
            .ReduceOrThrow();

        private string Link(Guid id) =>
            Url.Link("get-flight", new {id = id.ToString()});
    }
}