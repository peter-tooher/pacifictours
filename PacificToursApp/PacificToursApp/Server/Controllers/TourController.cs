using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace PacificToursApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase

    {
        private static List<Tour> Tours = new List<Tour>
        {
            new Tour
            {
                TourId = 1,
                TourName = "Real Britain",
                TourDescription = "Discover the heart of England, Wales, and Scotland",
                TourImageUrl = "https://www.thewowstyle.com/wp-content/uploads/2015/02/1653100london.jpg",
                TourLength = 6,
                TourPrice = 1200.00m,
                TourAvailableSpaces = 30,
            },
            new Tour
            {
                TourId = 2,
                TourName = "Britain and Ireland Explorer",
                TourDescription = "Venture across England, Wales, Scotland, and Ireland",
                TourImageUrl = "https://static.standard.co.uk/s3fs-public/thumbnails/image/2017/07/10/10/shutterstock-521968378.jpg?crop=8:5,smart&quality=75&auto=webp&width=1024",
                TourLength = 16,
                TourPrice = 2000.00m,
                TourAvailableSpaces = 40,
            },
            new Tour
            {
                TourId = 3,
                TourName = "Best of Britain",
                TourDescription = "Visit the tourist attractions throughout England, Wales, and Scotland",
                TourImageUrl = "https://www.english-heritage.org.uk/siteassets/home/visit/places-to-visit/stonehenge/history/stonehenge-aerial-1440x612.jpg?w=1440&h=612&mode=crop&scale=both&quality=100&anchor=NoFocus&WebsiteVersion=20231208103628",
                TourLength = 12,
                TourPrice = 2900.00m,
                TourAvailableSpaces = 30,
            }
        };

        [HttpGet]
        public async Task<ActionResult<List<Tour>>> GetTour()
        {
            return(Ok(Tours));
        }
    }
}
