using EventMiniApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventMiniApp.Data.Configurations
{
    public class EventConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.Location)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.BannerImageUrl)
                .HasMaxLength(500);

            builder.HasOne(x => x.Organizer)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.OrganizerId);

            builder.HasMany(x => x.Tickets)
                .WithOne(x => x.Event)
                .HasForeignKey(x => x.EventId);
        }
    }
}
