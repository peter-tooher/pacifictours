using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Filters;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace PacificToursApp.Server.Controllers
{
    // Define the route for the controller and specify that it's an API controller
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // Declare a private readonly field for the authentication service
        private readonly IAuthService _authSevice;

        // Inject the authentication service into the controller via the constructor
        public AuthController(IAuthService authSevice)
        {
            _authSevice = authSevice;
        }

        // Define a POST endpoint for user registration
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register([FromBody] UserRegister request)
        {
            // Call the Register method of the authentication service
            var response = await _authSevice.Register(request);

            // If the registration was not successful, return a BadRequest response
            if (!response.Success)
            {
                return BadRequest(response);
            }
            // If the registration was successful, return an Ok response
            return Ok(response);
        }

        // Define a POST endpoint for user login
        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login([FromBody] UserLogin request)
        {
            // Call the Login method of the authentication service
            var response = await _authSevice.Login(request.UserName, request.Password);

            // If the login was not successful, return a BadRequest response
            if (!response.Success)
            {
                return BadRequest(response);
            }
            // If the login was successful, return an Ok response
            return Ok(response);
        }

        // Define a POST endpoint for changing password, which requires authorization
        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword([FromBody] string newPassword)
        {
            // Get the user's ID from the claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Call the ChangePassword method of the authentication service
            var response = await _authSevice.ChangePassword(int.Parse(userId), newPassword);

            // If the password change was not successful, return a BadRequest response
            if (!response.Success)
            {
                return BadRequest(response);
            }
            // If the password change was successful, return an Ok response
            return Ok(response);
        }
    }
}
