using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // The PackageBookings class is a model that represents a package booking in the system.
    public class PackageBookings
    {
        // The BookingId property uniquely identifies each package booking.
        public int BookingId { get; set; }

        // The UserId property is a foreign key that identifies the user who made the package booking.
        public int UserId { get; set; }

        // The TourId property is a foreign key that identifies the tour included in the package booking.
        public int TourId { get; set; }

        // The TourName property represents the name of the tour included in the package booking.
        public string TourName { get; set; }

        // The TourCheckIn property represents the date and time of the tour included in the package booking.
        public DateTime TourCheckIn { get; set; }

        // The TourDeposit property represents the deposit amount for the tour included in the package booking.
        public decimal TourDeposit { get; set; }

        // The TourPrice property represents the total price of the tour included in the package booking.
        public decimal TourPrice { get; set; }

        // The TourPaymentDue property represents the date by which the payment for the tour should be made.
        public DateTime TourPaymentDue { get; set; }

        // The HotelId property is a foreign key that identifies the hotel included in the package booking.
        public int HotelId { get; set; }

        // The HotelName property represents the name of the hotel included in the package booking.
        public string HotelName { get; set; }

        // The SuiteOption property represents the suite option chosen for the hotel included in the package booking.
        public string SuiteOption { get; set; }

        // The HotelCheckIn and HotelCheckOut properties represent the check-in and check-out dates for the hotel included in the package booking.
        public DateTime HotelCheckIn { get; set; }
        public DateTime HotelCheckOut { get; set; }

        // The HotelDeposit property represents the deposit amount for the hotel included in the package booking.
        public decimal HotelDeposit { get; set; }

        // The HotelPrice property represents the total price of the hotel included in the package booking.
        public decimal HotelPrice { get; set; }

        // The HotelPaymentDue property represents the date by which the payment for the hotel should be made.
        public DateTime HotelPaymentDue { get; set; }

        // The Paid property indicates whether the package booking has been paid for. By default, it is false.
        public bool Paid { get; set; } = false;

        // The TotalPrice property represents the total price of the package booking.
        public decimal TotalPrice { get; set; }

        // The user, hotel, and tour properties are navigation properties that can be used to access the User, Hotel, and Tour objects associated with a package booking.
        // They are nullable.
        public User? user { get; set; }
        public Hotel? hotel { get; set; }
        public Tour? tour { get; set; }
    }
}
