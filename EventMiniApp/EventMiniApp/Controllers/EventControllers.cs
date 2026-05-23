using AutoMapper;
using EventMiniApp.Data;
using EventMiniApp.Dtos.EventDto;
using EventMiniApp.Dtos.OrganizerDto;
using EventMiniApp.Dtos.TicketDto;

using EventMiniApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventMiniApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly EventMiniAppDbContext _context;
        private readonly IMapper _mapper;

        public EventsController(EventMiniAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/events
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var events = await _context.Events
                .Include(e => e.Organizer)
                .Include(e => e.Tickets)
                .ToListAsync();

            var result = _mapper.Map<List<EventGetDto>>(events);

            return Ok(result);
        }

        // GET: api/events/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ev = await _context.Events
                .Include(e => e.Organizer)
                .Include(e => e.Tickets)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ev == null)
                return NotFound();

            return Ok(_mapper.Map<EventGetDto>(ev));
        }

        // POST: api/events
        [HttpPost]
        public async Task<IActionResult> Create(EventCreateDto dto)
        {
            var ev = _mapper.Map<Event>(dto);

            _context.Events.Add(ev);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<EventGetDto>(ev));
        }

        // PUT: api/events/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, EventUpdateDto dto)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null)
                return NotFound();

            _mapper.Map(dto, ev);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<EventGetDto>(ev));
        }

        // DELETE: api/events/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null)
                return NotFound();

            _context.Events.Remove(ev);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }

        // GET: api/events/{eventId}/tickets
        [HttpGet("{eventId}/tickets")]
        public async Task<IActionResult> GetTickets(int eventId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.EventId == eventId)
                .ToListAsync();

            return Ok(_mapper.Map<List<TicketGetDto>>(tickets));
        }

        // GET: api/events/{eventId}/organizer
        [HttpGet("{eventId}/organizer")]
        public async Task<IActionResult> GetOrganizer(int eventId)
        {
            var ev = await _context.Events
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(x => x.Id == eventId);

            if (ev == null)
                return NotFound();

            return Ok(_mapper.Map<OrganizerGetDto>(ev.Organizer));
        }

        // POST: api/events/{eventId}/tickets
        [HttpPost("{eventId}/tickets")]
        public async Task<IActionResult> CreateTicket(int eventId, TicketCreateDto dto)
        {
            var ev = await _context.Events.FindAsync(eventId);

            if (ev == null)
                return NotFound("Event not found");

            var ticket = _mapper.Map<Ticket>(dto);
            ticket.EventId = eventId;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<TicketGetDto>(ticket));
        }

        // POST: api/events/{id}/banner
        [HttpPost("{id}/banner")]
        public async Task<IActionResult> UploadBanner(int id, IFormFile file)
        {
            var ev = await _context.Events.FindAsync(id);

            if (ev == null)
                return NotFound();

            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine("wwwroot/banners", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            ev.BannerImageUrl = "/banners/" + fileName;

            await _context.SaveChangesAsync();

            return Ok(new { ev.BannerImageUrl });
        }
    }
}