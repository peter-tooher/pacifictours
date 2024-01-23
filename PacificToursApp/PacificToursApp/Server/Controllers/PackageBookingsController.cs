using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PacificToursApp.Server.Data;

namespace PacificToursApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageBookingsController : ControllerBase
    {
        private readonly DataContext _context;

        public PackageBookingsController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<PackageBookings>> PostPackageBooking(PackageBookings packageBooking)
        {
            _context.PackageBookings.Add(packageBooking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPackageBooking", new { id = packageBooking.BookingId }, packageBooking);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PackageBookings>> GetPackageBooking(int id)
        {
            var packageBooking = await _context.PackageBookings.FindAsync(id);

            if (packageBooking == null)
            {
                return NotFound();
            }

            return packageBooking;
        }

        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<PackageBookings>>> GetPackageBookingsByUser(int userId)
        {
            var packageBookings = await _context.PackageBookings.Where(b => b.UserId == userId).ToListAsync();

            if (!packageBookings.Any())
            {
                return NotFound();
            }

            return packageBookings;
        }

        [HttpPost("Pay/{id}")]
        public ActionResult Pay(int id)
        {
            var booking = _context.PackageBookings.FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            if (DateTime.Now > booking.TourPaymentDue || DateTime.Now > booking.HotelPaymentDue)
            {
                booking.Paid = true;
                booking.TourDeposit = 0;
                booking.HotelDeposit = 0;
                _context.SaveChanges();
                return Ok("Deposits have been forfeited due to late payment.");
            }

            booking.Paid = true;
            _context.SaveChanges();
            return Ok("Payment successful.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<decimal>> CancelPackageBooking(int id)
        {
            var booking = await _context.PackageBookings.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            var totalPrice = booking.TourPrice + booking.HotelPrice + booking.TourDeposit + booking.HotelDeposit;
            var daysToCommencement = Math.Min((booking.TourCheckIn - DateTime.Now).TotalDays, (booking.HotelCheckIn - DateTime.Now).TotalDays);
            var refund = daysToCommencement >= 5 ? totalPrice : totalPrice - 50;

            _context.PackageBookings.Remove(booking);
            await _context.SaveChangesAsync();

            return refund;
        }

        [HttpPut("{id}")]
        public ActionResult ModifyPackageBooking(int id, [FromBody] PackageBookings modifiedBooking)
        {
            var booking = _context.PackageBookings.FirstOrDefault(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            var daysBeforeBooking = Math.Min((booking.TourCheckIn - DateTime.Now).TotalDays, (booking.HotelCheckIn - DateTime.Now).TotalDays);

            if (daysBeforeBooking < 14)
            {
                return BadRequest("Package bookings can only be modified up to 14 days before the booking.");
            }

            decimal tourPrice = booking.TourPrice;
            decimal hotelPrice = booking.HotelPrice;
            decimal tourDeposit = tourPrice * 0.2m;
            decimal hotelDeposit = hotelPrice * 0.2m;
            decimal surcharge = (tourPrice + hotelPrice) * 0.05m;
            decimal totalPrice = tourPrice + hotelPrice + surcharge;
            decimal totalPriceWithoutDeposit = totalPrice - tourDeposit - hotelDeposit;

            booking.TourCheckIn = modifiedBooking.TourCheckIn;
            booking.HotelCheckIn = modifiedBooking.HotelCheckIn;
            booking.HotelCheckOut = modifiedBooking.HotelCheckOut;
            booking.TourPrice = totalPriceWithoutDeposit;
            booking.HotelPrice = totalPriceWithoutDeposit;
            booking.TourDeposit = tourDeposit;
            booking.HotelDeposit = hotelDeposit;
            booking.TourPaymentDue = CalculateTourPaymentDueDate(modifiedBooking.TourCheckIn);
            booking.HotelPaymentDue = CalculateHotelPaymentDueDate(modifiedBooking.HotelCheckIn);
            booking.TotalPrice = totalPrice;

            _context.SaveChanges();

            return Ok("Package booking modified successfully.");
        }

        private DateTime CalculateTourPaymentDueDate(DateTime tourCheckIn)
        {
            return tourCheckIn.AddDays(-28);
        }

        private DateTime CalculateHotelPaymentDueDate(DateTime hotelCheckIn)
        {
            return hotelCheckIn.AddDays(-28);
        }
    }
}