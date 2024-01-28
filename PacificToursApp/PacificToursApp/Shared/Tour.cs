using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacificToursApp.Shared
{
    // The Tour class is a model that represents a tour in the system.
    public class Tour
    {
        // The TourId property uniquely identifies each tour.
        public int TourId { get; set; }

        // The TourName property represents the name of the tour.
        public string TourName { get; set; } = string.Empty;

        // The TourDescription property provides a description of the tour.
        public string TourDescription { get; set; } = string.Empty;

        // The TourImageUrl property is a URL to an image that represents the tour.
        public string TourImageUrl { get; set; } = string.Empty;

        // The TourLength property represents the length of the tour in days.
        public int TourLength { get; set; }

        // The TourPrice property represents the price of the tour. It is a decimal with a precision of 18 and a scale of 2.
        [Column(TypeName = "decimal(18,2)")]
        public decimal TourPrice { get; set; }

        // The TourSpacesAvailable property represents the number of spaces available for the tour.
        public int TourSpacesAvailable { get; set; }
    }
}
