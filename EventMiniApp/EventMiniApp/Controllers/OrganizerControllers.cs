using AutoMapper;
using EventMiniApp.Data;
using EventMiniApp.Dtos.OrganizerDto;

using EventMiniApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventMiniApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrganizersController : ControllerBase
    {
        private readonly EventMiniAppDbContext _context;
        private readonly IMapper _mapper;

        public OrganizersController(EventMiniAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/organizers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var organizers = await _context.Organizers
                .Include(o => o.Events)
                .ToListAsync();

            var result = _mapper.Map<List<OrganizerGetDto>>(organizers);
            return Ok(result);
        }

        // GET: api/organizers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var organizer = await _context.Organizers
                .Include(o => o.Events)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (organizer == null)
                return NotFound();

            return Ok(_mapper.Map<OrganizerGetDto>(organizer));
        }

        // POST: api/organizers
        [HttpPost]
        public async Task<IActionResult> Create(OrganizerCreateDto dto)
        {
            var organizer = _mapper.Map<Organizer>(dto);

            _context.Organizers.Add(organizer);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<OrganizerGetDto>(organizer));
        }

        // PUT: api/organizers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, OrganizerUpdateDto dto)
        {
            var organizer = await _context.Organizers.FindAsync(id);

            if (organizer == null)
                return NotFound();

            _mapper.Map(dto, organizer);

            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<OrganizerGetDto>(organizer));
        }

        // DELETE: api/organizers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var organizer = await _context.Organizers.FindAsync(id);

            if (organizer == null)
                return NotFound();

            _context.Organizers.Remove(organizer);
            await _context.SaveChangesAsync();

            return Ok("Deleted");
        }

        // POST: api/organizers/{id}/logo
        [HttpPost("{id}/logo")]
        public async Task<IActionResult> UploadLogo(int id, IFormFile file)
        {
            var organizer = await _context.Organizers.FindAsync(id);

            if (organizer == null)
                return NotFound();

            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var path = Path.Combine("wwwroot/logos", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            organizer.LogoUrl = "/logos/" + fileName;

            await _context.SaveChangesAsync();

            return Ok(new { organizer.LogoUrl });
        }

        // GET: api/organizers/{id}/events
        [HttpGet("{id}/events")]
        public async Task<IActionResult> GetEvents(int id)
        {
            var organizer = await _context.Organizers
                .Include(o => o.Events)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (organizer == null)
                return NotFound();

            return Ok(organizer.Events);
        }
    }
}