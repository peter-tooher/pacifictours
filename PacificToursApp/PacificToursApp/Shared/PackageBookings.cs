using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    public class PackageBookings
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int TourId { get; set; }
        public string TourName { get; set; }
        public DateTime TourCheckIn { get; set; }
        public decimal TourDeposit { get; set; }
        public decimal TourPrice { get; set; }
        public DateTime TourPaymentDue { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string SuiteOption { get; set; }
        public DateTime HotelCheckIn { get; set; }
        public DateTime HotelCheckOut { get; set; }
        public decimal HotelDeposit { get; set; }
        public decimal HotelPrice { get; set; }
        public DateTime HotelPaymentDue { get; set; }
        public bool Paid { get; set; } = false;
        public decimal TotalPrice { get; set; }
        public User? user { get; set; }
        public Hotel? hotel { get; set; }
        public Tour? tour { get; set; }
    }
}
