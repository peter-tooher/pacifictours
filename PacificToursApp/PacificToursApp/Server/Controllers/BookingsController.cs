using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace PacificToursApp.Server.Controllers
{
    // The BookingsController class is a controller for managing hotel bookings. It inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        // The _context field is a DataContext instance used to interact with the database.
        // The _hotelService field is an IHotelService instance used to interact with the hotel service.
        private readonly DataContext _context;
        private readonly IHotelService _hotelService;
        private decimal totalPrice;

        // The BookingsController constructor takes a DataContext and IHotelService instance and assigns them to the _context and _hotelService fields.
        public BookingsController(DataContext context, IHotelService hotelService)
        {
            _context = context;
            _hotelService = hotelService;
        }

        // The GetAllHotelBookings method retrieves all hotel bookings.
        [HttpGet("GetAllHotelBookings")]
        public async Task<ActionResult<IEnumerable<HotelBookings>>> GetAllHotelBookings()
        {
            return await _context.HotelBookings.ToListAsync();
        }

        // The DeleteBooking method deletes a booking.
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

        // The PostBooking method creates a new booking.
        [HttpPost]
        public ActionResult<HotelBookings> PostBooking([FromBody] HotelBookings booking)
        {
            _context.HotelBookings.Add(booking);
            _context.SaveChanges();

            return CreatedAtAction("GetBooking", new { id = booking.BookingId }, booking);
        }

        // The GetBookings method retrieves bookings by user.
        [HttpGet("{userId}")]
        public ActionResult<IEnumerable<HotelBookings>> GetBookings(int userId)
        {
            return _context.HotelBookings.Where(b => b.UserId == userId).ToList();
        }

        // The GetBooking method retrieves a booking by its ID.
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

        // The GetBookingsByHotelAndSuite method retrieves bookings by hotel and suite.
        [HttpGet("{hotelId}/{suiteType}")]
        public ActionResult<IEnumerable<HotelBookings>> GetBookingsByHotelAndSuite(int hotelId, string suiteType)
        {
            return _context.HotelBookings.Where(b => b.HotelId == hotelId && b.SuiteOption == suiteType).ToList();
        }

        // The Pay method marks a booking as paid.
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

        // The Modify method modifies a booking.
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

        // The CalculateDeposit method calculates the deposit for a booking.
        private decimal CalculateDeposit(HotelBookings booking)
        {
            return booking.Price * 0.2m;
        }

        // The CalculatePaymentDueDate method calculates the payment due date for a booking.
        private DateTime CalculatePaymentDueDate(HotelBookings booking)
        {
            return booking.CheckIn.AddDays(-28);
        }
    }
}
