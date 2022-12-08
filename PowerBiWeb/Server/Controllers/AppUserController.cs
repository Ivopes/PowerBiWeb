using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PowerBiWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUserController : ControllerBase
    {
        private readonly IAppUserService _appUserService;

        public AppUserController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }

        // GET: api/<AppUserController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplUser>>> GetAsync()
        {
            return Ok(await _appUserService.GetAsync());
        }

        // GET api/<AppUserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AppUserController>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] UserRegisterInformation user)
        {
            var response = await _appUserService.PostAsync(user);
            if (string.IsNullOrEmpty(response)) return Ok();

            return BadRequest(response);
        }

        // PUT api/<AppUserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AppUserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
