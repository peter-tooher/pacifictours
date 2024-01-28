using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PacificToursApp.Server.Controllers
{
    // The HotelController class is a controller for managing hotels. It inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        // The _hotelService field is an IHotelService instance used to interact with the hotel service.
        private readonly IHotelService _hotelService;

        // The HotelController constructor takes an IHotelService instance and assigns it to the _hotelService field.
        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // The GetHotels method retrieves all hotels.
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Hotel>>>> GetHotels()
        {
            // The GetHotelsAsync method of the hotel service is called to retrieve all hotels.
            var result = await _hotelService.GetHotelsAsync();
            // The result is returned with a 200 OK status.
            return (Ok(result));
        }

        // The GetHotelById method retrieves a hotel by its ID.
        [HttpGet("{hotelId}")]
        public async Task<ActionResult<ServiceResponse<Hotel>>> GetHotelById([FromRoute] int hotelId)
        {
            // The GetHotelByIdAsync method of the hotel service is called to retrieve the hotel with the specified ID.
            var result = await _hotelService.GetHotelByIdAsync(hotelId);
            // The result is returned with a 200 OK status.
            return (Ok(result));
        }
    }
}