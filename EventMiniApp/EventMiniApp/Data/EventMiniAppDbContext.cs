using EventMiniApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventMiniApp.Data
{
    public class EventMiniAppDbContext : DbContext
    {

        public DbSet<Event> Events { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public EventMiniAppDbContext(DbContextOptions<EventMiniAppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventMiniAppDbContext).Assembly);
        }
    }
}
