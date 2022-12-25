using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Shared.User;

namespace PowerBiWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<ActionResult<UserLoginInformation>> Login([FromBody] UserLoginInformation user)
        {
            var result = await _authService.LoginAsync(user);

            if (!string.IsNullOrEmpty(result)) return Ok(result);


            return Unauthorized();
        }
    }
}
