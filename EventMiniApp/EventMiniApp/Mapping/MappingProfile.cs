using AutoMapper;
using EventMiniApp.Dtos.EventDto;
using EventMiniApp.Dtos.OrganizerDto;
using EventMiniApp.Dtos.TicketDto;

using EventMiniApp.Models;

namespace EventMiniApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // EVENT
            CreateMap<EventCreateDto, Event>();
            CreateMap<EventUpdateDto, Event>();
            CreateMap<Event, EventGetDto>();

            // ORGANIZER
            CreateMap<OrganizerCreateDto, Organizer>();
            CreateMap<OrganizerUpdateDto, Organizer>();
            CreateMap<Organizer, OrganizerGetDto>();

            // TICKET
            CreateMap<TicketCreateDto, Ticket>();
            CreateMap<TicketUpdateDto, Ticket>();
            CreateMap<Ticket, TicketGetDto>();
        }
    }
}