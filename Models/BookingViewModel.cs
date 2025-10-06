namespace CalendarBookingSystem.Models
{
    public class BookingViewModel
    {
        public DateTime CurrentDate { get; set; } = DateTime.Today;
        public List<Booking> Bookings { get; set; } = new List<Booking>();
        public List<TimeSlot> AvailableTimeSlots { get; set; } = new List<TimeSlot>();
        public int CurrentMonth => CurrentDate.Month;
        public int CurrentYear => CurrentDate.Year;
        
        public List<DateTime> GetCalendarDays()
        {
            var firstDayOfMonth = new DateTime(CurrentYear, CurrentMonth, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var startDate = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
            var endDate = lastDayOfMonth.AddDays(6 - (int)lastDayOfMonth.DayOfWeek);
            
            var days = new List<DateTime>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                days.Add(date);
            }
            return days;
        }
        
        public List<Booking> GetBookingsForDate(DateTime date)
        {
            return Bookings.Where(b => b.Date.Date == date.Date).ToList();
        }
    }
    
    public class TimeSlot
    {
        public TimeSpan Time { get; set; }
        public bool IsAvailable { get; set; }
        public string DisplayTime => Time.ToString(@"hh\:mm");
    }
}