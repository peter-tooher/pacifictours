using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        public required SingleSuite SingleSuite { get; set; }
        public required DoubleSuite DoubleSuite { get; set; }    
        public required FamilySuite FamilySuite { get; set; }
    }
    public class SingleSuite
    {
        public int SingleSuiteId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal SingleSuitePrice { get; set; }
        public int SingleSuiteAvailableSpaces { get; set; }
    }
    public class DoubleSuite
    {
        public int DoubleSuiteId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DoubleSuitePrice { get; set; }
        public int DoubleSuiteAvailableSpaces { get; set; }
    }
    public class FamilySuite
    {
        public int FamilySuiteId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal FamilySuitePrice { get; set; }
        public int FamilySuiteAvailableSpaces { get; set; }
    }
}