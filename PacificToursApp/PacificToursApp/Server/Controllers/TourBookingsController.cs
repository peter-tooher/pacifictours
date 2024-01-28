using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PacificToursApp.Server.Controllers
{
    // The TourBookingsController class is a controller for managing tour bookings. It inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class TourBookingsController : ControllerBase
    {
        // The _context field is a DataContext instance used to interact with the database.
        // The _tourService field is an ITourService instance used to interact with the tour service.
        private readonly DataContext _context;
        private readonly ITourService _tourService;
        private decimal totalPrice;

        // The TourBookingsController constructor takes a DataContext and ITourService instance and assigns them to the _context and _tourService fields.
        public TourBookingsController(DataContext context, ITourService tourService)
        {
            _context = context;
            _tourService = tourService;
        }

        // The GetAllTourBookings method retrieves all tour bookings.
        [HttpGet("GetAllTourBookings")]
        public async Task<ActionResult<IEnumerable<TourBookings>>> GetAllTourBookings()
        {
            return await _context.TourBookings.ToListAsync();
        }

        // The DeleteBooking method deletes a booking.
        [HttpDelete("{id}")]
        public async Task<ActionResult<decimal>> DeleteBooking(int id)
        {
            var booking = await _context.TourBookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            totalPrice = booking.Price + booking.Deposit;
            var daysToCommencement = (booking.CheckIn - DateTime.Now).TotalDays;
            var refund = daysToCommencement >= 5 ? totalPrice : totalPrice - 50;

            _context.TourBookings.Remove(booking);
            await _context.SaveChangesAsync();

            return refund;
        }

        // The GetBookingsByTourAndDate method retrieves bookings by tour and date.
        [HttpGet("ByTourAndDate/{tourId}/{date}")]
        public ActionResult<IEnumerable<TourBookings>> GetBookingsByTourAndDate(int tourId, DateTime date)
        {
            return _context.TourBookings.Where(b => b.TourId == tourId && b.CheckIn.Date == date).ToList();
        }

        // The PostBooking method creates a new booking.
        [HttpPost]
        public ActionResult<TourBookings> PostBooking([FromBody] TourBookings booking)
        {
            int maxSpaces = (_context.Tours.FirstOrDefault(t => t.TourId == booking.TourId).TourName == "Real Britain" || _context.Tours.FirstOrDefault(t => t.TourId == booking.TourId).TourName == "Best of Britain") ? 30 : 40;
            int bookingsOnDate = _context.TourBookings.Count(b => b.TourId == booking.TourId && b.CheckIn.Date == booking.CheckIn.Date);

            if (bookingsOnDate >= maxSpaces)
            {
                return BadRequest("Sorry, there are no available spaces for this tour on the selected date.");
            }

            _context.TourBookings.Add(booking);
            _context.SaveChanges();

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        // The GetBookings method retrieves bookings by user.
        [HttpGet("ByUser/{userId}")]
        public ActionResult<IEnumerable<TourBookings>> GetBookings(int userId)
        {
            return _context.TourBookings.Where(b => b.UserId == userId).ToList();
        }

        // The GetBooking method retrieves a booking by its ID.
        [HttpGet("GetBooking/{id}")]
        public ActionResult<TourBookings> GetBooking(int id)
        {
            var booking = _context.TourBookings.FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        // The GetBookingsByTour method retrieves bookings by tour.
        [HttpGet("ByTour/{tourId}")]
        public ActionResult<IEnumerable<TourBookings>> GetBookingsByTour(int tourId)
        {
            return _context.TourBookings.Where(b => b.TourId == tourId).ToList();
        }

        // The Pay method marks a booking as paid.
        [HttpPost("Pay/{id}")]
        public ActionResult Pay(int id)
        {
            var booking = _context.TourBookings.FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            if (DateTime.Now > booking.PaymentDue)
            {
                booking.Paid = true;
                booking.Deposit = 0;
                _context.SaveChanges();
                return Ok("Deposit has been forfeited due to late payment.");
            }

            booking.Paid = true;
            _context.SaveChanges();
            return Ok("Payment successful.");
        }

        // The Modify method modifies a booking.
        [HttpPut("{id}")]
        public ActionResult Modify(int id, [FromBody] TourBookings modifiedBooking)
        {
            var booking = _context.TourBookings.FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            var daysBeforeBooking = (booking.CheckIn - DateTime.Now).TotalDays;

            if (daysBeforeBooking < 14)
            {
                return BadRequest("Bookings can only be modified up to 14 days before the booking.");
            }

            var tour = _tourService.GetTourByIdAsync(booking.TourId).Result.Data;

            decimal price = tour.TourPrice;
            decimal Deposit = tour.TourPrice * 0.2m;
            decimal surcharge = tour.TourPrice * 0.05m;
            decimal totalPrice = tour.TourPrice + surcharge;
            decimal totalPriceWithoutDeposit = totalPrice - Deposit;

            booking.CheckIn = modifiedBooking.CheckIn;
            booking.Price = totalPriceWithoutDeposit;
            booking.Deposit = Deposit;
            booking.PaymentDue = CalculatePaymentDueDate(modifiedBooking);

            _context.SaveChanges();

            return Ok("Booking modified successfully.");
        }

        // The CalculateDeposit method calculates the deposit for a booking.
        private decimal CalculateDeposit(TourBookings booking)
        {
            return booking.Price * 0.2m;
        }

        // The CalculatePaymentDueDate method calculates the payment due date for a booking.
        private DateTime CalculatePaymentDueDate(TourBookings booking)
        {
            return booking.CheckIn.AddDays(-28);
        }
    }
}