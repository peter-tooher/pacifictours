using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace PacificToursApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TourController : ControllerBase

    {
        private readonly ITourService _tourService;

        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }   

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Tour>>>> GetTours()
        {
            var result = await _tourService.GetToursAsync();
            return(Ok(result));
        }

        [HttpGet("{tourId}")]
        public async Task<ActionResult<ServiceResponse<Tour>>> GetTourById([FromRoute] int tourId)
        {
            var result = await _tourService.GetTourByIdAsync(tourId);
            return (Ok(result));
        }
    }
}
