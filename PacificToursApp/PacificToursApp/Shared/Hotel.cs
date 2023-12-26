using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    public class Hotel
    {
        public int HotelId { get; set; }
        public string HotelName { get; set; } = string.Empty;
        public string HotelDescription { get; set; } = string.Empty;
        public string HotelImageUrl { get; set; } = string.Empty;
        
        public required Suite SingleSuite { get; set; }
        public required Suite DoubleSuite { get; set; }
        public required Suite FamilySuite { get; set; }
    }

    public class Suite
    { 
        public int SuiteId { get; set; }
        public decimal SuitePrice { get; set; }
        public int SuiteAvailableSpaces { get; set; }
    }
}
