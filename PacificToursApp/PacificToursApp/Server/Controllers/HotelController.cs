using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PacificToursApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;
        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Hotel>>>> GetHotels()
        {
            var result = await _hotelService.GetHotelsAsync();
            return (Ok(result));
        }

        [HttpGet("{hotelId}")]
        public async Task<ActionResult<ServiceResponse<Hotel>>> GetHotelById([FromRoute] int hotelId)
        {
            var result = await _hotelService.GetHotelByIdAsync(hotelId);
            return (Ok(result));
        }
    }
}