using AirView.Application;
using AutoMapper;
using Humanizer;

namespace AirView.Api.Flights
{
    public class FlightMappingProfile : Profile
    {
        public FlightMappingProfile()
        {
            CreateMap<ScheduleFlightDto, ScheduleFlightCommand>()
                .ForCtorParam(nameof(ScheduleFlightCommand.Id).Camelize(),
                    opt => opt.ResolveUsing((src, ctx) => ctx.Options.Items[nameof(ScheduleFlightCommand.Id)]));
        }
    }
}