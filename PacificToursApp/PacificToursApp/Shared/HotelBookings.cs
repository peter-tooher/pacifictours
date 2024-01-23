using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    public class HotelBookings
    {
        public int BookingId { get; set; }
        public int UserId { get; set; }
        public int HotelId { get; set; }
        public string HotelName { get; set; }
        public string SuiteOption { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Deposit { get; set; }
        public decimal Price { get; set; }
        public DateTime PaymentDue { get; set; }
        public bool Paid { get; set; } = false;
        public User? user { get; set; }
        public Hotel? hotel { get; set; }
    }
}
