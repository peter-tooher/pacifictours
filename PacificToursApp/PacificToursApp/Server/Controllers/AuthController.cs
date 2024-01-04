using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PacificToursApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authSevice;

        public AuthController(IAuthService authSevice)
        {
            _authSevice = authSevice;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register([FromBody]UserRegister request)
        {
            var response = await _authSevice.Register(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login([FromBody]UserLogin request)
        {
            var response = await _authSevice.Login(request.UserName, request.Password);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
