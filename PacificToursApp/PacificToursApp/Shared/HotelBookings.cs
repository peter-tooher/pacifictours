using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // The HotelBookings class is a model that represents a hotel booking in the system.
    public class HotelBookings
    {
        // The BookingId property uniquely identifies each hotel booking.
        public int BookingId { get; set; }

        // The UserId property is a foreign key that identifies the user who made the hotel booking.
        public int UserId { get; set; }

        // The HotelId property is a foreign key that identifies the hotel that was booked.
        public int HotelId { get; set; }

        // The HotelName property represents the name of the hotel that was booked.
        public string HotelName { get; set; }

        // The SuiteOption property represents the suite option chosen for the hotel booking.
        public string SuiteOption { get; set; }

        // The CheckIn and CheckOut properties represent the check-in and check-out dates for the hotel booking.
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }

        // The Deposit property represents the deposit amount for the hotel booking.
        public decimal Deposit { get; set; }

        // The Price property represents the total price of the hotel booking.
        public decimal Price { get; set; }

        // The PaymentDue property represents the date by which the payment for the hotel booking should be made.
        public DateTime PaymentDue { get; set; }

        // The Paid property indicates whether the hotel booking has been paid for. By default, it is false.
        public bool Paid { get; set; } = false;

        // The user and hotel properties are navigation properties that can be used to access the User and Hotel objects associated with a hotel booking.
        // They are nullable.
        public User? user { get; set; }
        public Hotel? hotel { get; set; }
    }
}
