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

        [Column(TypeName = "decimal(18,2)")]
        public decimal SingleSuitePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DoubleSuitePrice { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal FamilySuitePrice { get; set; }
    }
}