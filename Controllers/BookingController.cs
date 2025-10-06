using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarBookingSystem.Data;
using CalendarBookingSystem.Models;

namespace CalendarBookingSystem.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingContext _context;

        public BookingController(BookingContext context)
        {
            _context = context;
            // Ensure database is created and seeded
            _context.Database.EnsureCreated();
        }

        // GET: Booking
        public async Task<IActionResult> Index(int? month, int? year)
        {
            var currentDate = new DateTime(year ?? DateTime.Now.Year, month ?? DateTime.Now.Month, 1);
            var bookings = await _context.Bookings.ToListAsync();

            var viewModel = new BookingViewModel
            {
                CurrentDate = currentDate,
                Bookings = bookings,
                AvailableTimeSlots = GetAvailableTimeSlots()
            };

            return View(viewModel);
        }

        // GET: Booking/Create
        public IActionResult Create(DateTime? date)
        {
            var booking = new Booking();
            if (date.HasValue)
            {
                booking.Date = date.Value.Date;
            }
            else
            {
                booking.Date = DateTime.Today;
            }

            ViewBag.AvailableTimeSlots = GetAvailableTimeSlots();
            return View(booking);
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Email,Date,Time,Description")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Check if time slot is already booked
                var existingBooking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.Date.Date == booking.Date.Date && b.Time == booking.Time);

                if (existingBooking != null)
                {
                    ModelState.AddModelError("Time", "This time slot is already booked. Please choose another time.");
                    ViewBag.AvailableTimeSlots = GetAvailableTimeSlots();
                    return View(booking);
                }

                booking.CreatedAt = DateTime.Now;
                _context.Add(booking);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Booking created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.AvailableTimeSlots = GetAvailableTimeSlots();
            return View(booking);
        }

        // GET: Booking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Booking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Booking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Booking deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // API endpoint to get available time slots for a specific date
        [HttpGet]
        public async Task<IActionResult> GetAvailableSlots(DateTime date)
        {
            var bookedTimes = await _context.Bookings
                .Where(b => b.Date.Date == date.Date)
                .Select(b => b.Time)
                .ToListAsync();

            var allSlots = GetAvailableTimeSlots();
            var availableSlots = allSlots
                .Where(slot => !bookedTimes.Contains(slot.Time))
                .ToList();

            return Json(availableSlots);
        }

        private List<TimeSlot> GetAvailableTimeSlots()
        {
            var slots = new List<TimeSlot>();
            
            // Generate time slots from 9 AM to 5 PM, every hour
            for (int hour = 9; hour <= 17; hour++)
            {
                slots.Add(new TimeSlot
                {
                    Time = new TimeSpan(hour, 0, 0),
                    IsAvailable = true
                });
            }

            return slots;
        }
    }
}