using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PacificToursApp.Server.Data;

namespace PacificToursApp.Server.Controllers
{
    // The PackageBookingsController class is a controller for managing package bookings. It inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class PackageBookingsController : ControllerBase
    {
        // The _context field is a DataContext instance used to interact with the database.
        private readonly DataContext _context;

        // The PackageBookingsController constructor takes a DataContext instance and assigns it to the _context field.
        public PackageBookingsController(DataContext context)
        {
            _context = context;
        }

        // The GetAllPackageBookings method retrieves all package bookings.
        [HttpGet("GetAllPackageBookings")]
        public async Task<ActionResult<IEnumerable<PackageBookings>>> GetAllPackageBookings()
        {
            return await _context.PackageBookings.ToListAsync();
        }

        // The PostPackageBooking method creates a new package booking.
        [HttpPost]
        public async Task<ActionResult<PackageBookings>> PostPackageBooking(PackageBookings packageBooking)
        {
            _context.PackageBookings.Add(packageBooking);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPackageBooking", new { id = packageBooking.BookingId }, packageBooking);
        }

        // The GetPackageBooking method retrieves a package booking by its ID.
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

        // The GetPackageBookingsByUser method retrieves package bookings by user.
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

        // The Pay method marks a package booking as paid.
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

        // The CancelPackageBooking method cancels a package booking.
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

        // The ModifyPackageBooking method modifies a package booking.
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

        // The CalculateTourPaymentDueDate method calculates the payment due date for a tour.
        private DateTime CalculateTourPaymentDueDate(DateTime tourCheckIn)
        {
            return tourCheckIn.AddDays(-28);
        }

        // The CalculateHotelPaymentDueDate method calculates the payment due date for a hotel.
        private DateTime CalculateHotelPaymentDueDate(DateTime hotelCheckIn)
        {
            return hotelCheckIn.AddDays(-28);
        }
    }
}