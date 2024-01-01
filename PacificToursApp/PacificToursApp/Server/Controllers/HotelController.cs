using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PacificToursApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly DataContext _context;
        public HotelController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHotels()
        {
            var hotels = await _context.Hotels.Include(h => h.SingleSuite).Include(h => h.DoubleSuite).Include(h => h.FamilySuite).ToListAsync();
            return (Ok(hotels));
        }
    }
}