using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PacificToursApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourBookingsController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ITourService _tourService;
        private decimal totalPrice;

        public TourBookingsController(DataContext context, ITourService tourService)
        {
            _context = context;
            _tourService = tourService;
        }

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

        [HttpGet("ByTourAndDate/{tourId}/{date}")]
        public ActionResult<IEnumerable<TourBookings>> GetBookingsByTourAndDate(int tourId, DateTime date)
        {
        return _context.TourBookings.Where(b => b.TourId == tourId && b.CheckIn.Date == date).ToList();
        }

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

        [HttpGet("ByUser/{userId}")]
        public ActionResult<IEnumerable<TourBookings>> GetBookings(int userId)
        {
            return _context.TourBookings.Where(b => b.UserId == userId).ToList();
        }

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

        [HttpGet("ByTour/{tourId}")]
        public ActionResult<IEnumerable<TourBookings>> GetBookingsByTour(int tourId)
        {
            return _context.TourBookings.Where(b => b.TourId == tourId).ToList();
        }

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

        private decimal CalculateDeposit(TourBookings booking)
        {
            return booking.Price * 0.2m;
        }

        private DateTime CalculatePaymentDueDate(TourBookings booking)
        {
            return booking.CheckIn.AddDays(-28);
        }
    }
}