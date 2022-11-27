using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public ActionResult<User> Login([FromBody] User user)
        {
            user.Password = "";
            user.Role = "Mama, Admin";
            return Ok(user);
        }
    }
}
