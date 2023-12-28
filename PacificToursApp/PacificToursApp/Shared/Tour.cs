using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    public class Tour
    {
        public int TourId { get; set; }
        public string TourName { get; set; } = string.Empty;
        public string TourDescription { get; set; } = string.Empty;
        public string TourImageUrl { get; set; } = string.Empty;
        public int TourLength { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TourPrice { get; set; }

        public int TourAvailableSpaces { get; set; }
    }
}
