using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PacificToursApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly DataContext _context; 
        private readonly IHotelService _hotelService;
        private decimal totalPrice;

        public BookingsController(DataContext context, IHotelService hotelService)
        {
            _context = context;
            _hotelService = hotelService;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<decimal>> DeleteBooking(int id)
        {
            var booking = await _context.HotelBookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            totalPrice = booking.Price + booking.Deposit;
            var daysToCommencement = (booking.CheckIn - DateTime.Now).TotalDays;
            var refund = daysToCommencement >= 5 ? totalPrice : totalPrice - 50;

            _context.HotelBookings.Remove(booking);
            await _context.SaveChangesAsync();

            return refund;
        }

        [HttpPost]
        public ActionResult<HotelBookings> PostBooking([FromBody] HotelBookings booking)
        {
            _context.HotelBookings.Add(booking);
            _context.SaveChanges();

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<HotelBookings>> GetBookings(int userId)
        {
            return _context.HotelBookings.Where(b => b.UserId == userId).ToList();
        }

        [HttpGet("GetBooking/{id}")]
        public ActionResult<HotelBookings> GetBooking(int id)
        {
            var booking = _context.HotelBookings.FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return booking;
        }

        [HttpGet("{hotelId}/{suiteType}")]
        public ActionResult<IEnumerable<HotelBookings>> GetBookingsByHotelAndSuite(int hotelId, string suiteType)
        {
            return _context.HotelBookings.Where(b => b.HotelId == hotelId && b.SuiteOption == suiteType).ToList();
        }

        [HttpPost("Pay/{id}")]
        public ActionResult Pay(int id)
        {
            var booking = _context.HotelBookings.FirstOrDefault(b => b.BookingId == id);

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
        public ActionResult Modify(int id, [FromBody] HotelBookings modifiedBooking)
        {
            var booking = _context.HotelBookings.FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            var daysBeforeBooking = (booking.CheckIn - DateTime.Now).TotalDays;

            if (daysBeforeBooking < 14)
            {
                return BadRequest("Bookings can only be modified up to 14 days before the booking.");
            }

            var hotel = _hotelService.GetHotelByIdAsync(booking.HotelId).Result.Data;

            decimal price = modifiedBooking.SuiteOption switch
            {
                "Single" => hotel.SingleSuitePrice,
                "Double" => hotel.DoubleSuitePrice,
                "Family" => hotel.FamilySuitePrice,
                _ => 0
            };

            int numberOfDays = (modifiedBooking.CheckOut - modifiedBooking.CheckIn).Days;
            decimal newPrice = price * numberOfDays;
            decimal newDeposit = newPrice * 0.2m;
            decimal surcharge = newPrice * 0.05m;
            decimal totalPrice = newPrice + surcharge;
            decimal totalPriceWithoutDeposit = totalPrice - newDeposit;

            booking.CheckIn = modifiedBooking.CheckIn;
            booking.CheckOut = modifiedBooking.CheckOut;
            booking.SuiteOption = modifiedBooking.SuiteOption;
            booking.Price = totalPriceWithoutDeposit;
            booking.Deposit = newDeposit;
            booking.PaymentDue = CalculatePaymentDueDate(modifiedBooking);

            _context.SaveChanges();

            return Ok("Booking modified successfully.");
        }

        private decimal CalculateDeposit(HotelBookings booking)
        {
            return booking.Price * 0.2m; 
        }

        private DateTime CalculatePaymentDueDate(HotelBookings booking)
        {
            return booking.CheckIn.AddDays(-28); 
        }
    }
}
