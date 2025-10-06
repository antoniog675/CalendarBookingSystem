using Microsoft.EntityFrameworkCore;
using CalendarBookingSystem.Models;

namespace CalendarBookingSystem.Data
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) : base(options)
        {
        }

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the Booking entity
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Description).HasMaxLength(500);
            });

            // Seed some sample data
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john@example.com",
                    Date = DateTime.Today.AddDays(1),
                    Time = new TimeSpan(9, 0, 0),
                    Description = "Initial consultation",
                    CreatedAt = DateTime.Now
                },
                new Booking
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane@example.com",
                    Date = DateTime.Today.AddDays(2),
                    Time = new TimeSpan(14, 0, 0),
                    Description = "Follow-up meeting",
                    CreatedAt = DateTime.Now
                }
            );
        }
    }
}