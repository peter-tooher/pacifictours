using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace PacificToursApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase

    {
        private readonly DataContext _context;

        public TourController(DataContext context)
        {
            _context = context;
        }   

        [HttpGet]
        public async Task<ActionResult<List<Tour>>> GetTour()
        {
            var tours = await _context.Tours.ToListAsync();
            return(Ok(tours));
        }
    }
}
