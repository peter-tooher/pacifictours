using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace PacificToursApp.Server.Controllers
{
    // The TourController class is a controller for managing tours. It inherits from ControllerBase.
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase
    {
        // The _tourService field is an ITourService instance used to interact with the tour service.
        private readonly ITourService _tourService;

        // The TourController constructor takes an ITourService instance and assigns it to the _tourService field.
        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        // The GetTours method retrieves all tours.
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Tour>>>> GetTours()
        {
            // The GetToursAsync method of the tour service is called to retrieve all tours.
            var result = await _tourService.GetToursAsync();
            // The result is returned with a 200 OK status.
            return Ok(result);
        }

        // The GetTourById method retrieves a tour by its ID.
        [HttpGet("{tourId}")]
        public async Task<ActionResult<ServiceResponse<Tour>>> GetTourById([FromRoute] int tourId)
        {
            // The GetTourByIdAsync method of the tour service is called to retrieve the tour with the specified ID.
            var result = await _tourService.GetTourByIdAsync(tourId);
            // The result is returned with a 200 OK status.
            return Ok(result);
        }
    }
}
