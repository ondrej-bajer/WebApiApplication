using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiApplication.DTOs.Auth;
using WebApiApplication.Interfaces;

namespace WebApiApplication.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<LoginResponse> Login([FromBody] LoginRequest req)
        {
            var result = _auth.Login(req);
            if (result is null)
                return Unauthorized(new { message = "Invalid credentials" });

            return Ok(result);
        }
    }
}
