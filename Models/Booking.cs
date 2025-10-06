using System.ComponentModel.DataAnnotations;

namespace CalendarBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        
        [Required]
        [Display(Name = "Time")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }
        
        [Display(Name = "Description")]
        public string? Description { get; set; }
        
        [Display(Name = "Created")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime GetFullDateTime() => Date.Add(Time);
    }
}