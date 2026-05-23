using AutoMapper;
using EventMiniApp.Data;
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
    public class TicketsController : ControllerBase
    {
        private readonly EventMiniAppDbContext _context;
        private readonly IMapper _mapper;

        public TicketsController(EventMiniAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tickets
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _context.Tickets
                .Include(t => t.Event)
                .ToListAsync();

            var result = _mapper.Map<List<TicketGetDto>>(tickets);

            return Ok(result);
        }

        // GET: api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ticket = await _context.Tickets
                .Include(t => t.Event)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ticket == null)
                return NotFound();

            return Ok(_mapper.Map<TicketGetDto>(ticket));
        }

        // POST: api/tickets
        [HttpPost]
        public async Task<IActionResult> Create(TicketCreateDto dto)
        {
            var eventExists = await _context.Events.AnyAsync(e => e.Id == dto.EventId);

            if (!eventExists)
                return NotFound("Event not found");

            var ticket = _mapper.Map<Ticket>(dto);

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<TicketGetDto>(ticket));
        }

        // PUT: api/tickets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TicketUpdateDto dto)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
                return NotFound();

            _mapper.Map(dto, ticket);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<TicketGetDto>(ticket));
        }

        // DELETE: api/tickets/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
                return NotFound();

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }
    }
}