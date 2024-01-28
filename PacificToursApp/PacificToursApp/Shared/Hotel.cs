using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // The Hotel class is a model that represents a hotel in the system.
    public class Hotel
    {
        // The HotelId property uniquely identifies each hotel.
        public int HotelId { get; set; }

        // The HotelName property represents the name of the hotel.
        public string HotelName { get; set; } = string.Empty;

        // The HotelDescription property provides a description of the hotel.
        public string HotelDescription { get; set; } = string.Empty;

        // The HotelImageUrl property is a URL to an image that represents the hotel.
        public string HotelImageUrl { get; set; } = string.Empty;

        // The SingleSuitePrice, DoubleSuitePrice, and FamilySuitePrice properties represent the prices of the single suite, double suite, and family suite respectively.
        // They are decimals with a precision of 18 and a scale of 2.
        [Column(TypeName = "decimal(18,2)")]
        public decimal SingleSuitePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DoubleSuitePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal FamilySuitePrice { get; set; }
    }
}