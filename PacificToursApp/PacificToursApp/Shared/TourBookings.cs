using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // The TourBookings class is a model that represents a tour booking in the system.
    public class TourBookings
    {
        // The BookingId property uniquely identifies each booking.
        public int BookingId { get; set; }

        // The UserId property is a foreign key that identifies the user who made the booking.
        public int UserId { get; set; }

        // The TourId property is a foreign key that identifies the tour that was booked.
        public int TourId { get; set; }

        // The TourName property represents the name of the tour that was booked.
        public string TourName { get; set; }

        // The CheckIn property represents the date and time of the tour.
        public DateTime CheckIn { get; set; }

        // The Deposit property represents the deposit amount for the booking.
        public decimal Deposit { get; set; }

        // The Price property represents the total price of the booking.
        public decimal Price { get; set; }

        // The PaymentDue property represents the date by which the payment should be made.
        public DateTime PaymentDue { get; set; }

        // The Paid property indicates whether the booking has been paid for. By default, it is false.
        public bool Paid { get; set; } = false;

        // The user property represents the user who made the booking. It is nullable.
        public User? user { get; set; }

        // The tour property represents the tour that was booked. It is nullable.
        public Tour? tour { get; set; }
    }
}
