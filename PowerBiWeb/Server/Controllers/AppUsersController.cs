﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Services;
using System.Security.Claims;
using PowerBiWeb.Shared.Users;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PowerBiWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly IAppUserService _appUserService;

        public AppUsersController(IAppUserService appUserService)
        {
            _appUserService = appUserService;
        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDetail>> GetByIdAsync(int id)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }
            if (userId != id)
            {
                return Forbid();
            }

            var result = await _appUserService.GetByIdAsync(id);

            if (result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] UserRegisterInformation user)
        {
            var response = await _appUserService.PostAsync(user);
            if (string.IsNullOrEmpty(response)) return Ok();

            return BadRequest(response);
        }
        [HttpPost("username/{newUsername}")]
        [Authorize]
        public async Task<ActionResult<UserDetail>> ChangeUsernameAsync([FromRoute] string newUsername)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }

            string response = await _appUserService.ChangeUsernameAsync(userId, newUsername);
            
            if (!string.IsNullOrEmpty(response)) return BadRequest(response);

            var newUser = await _appUserService.GetByIdAsync(userId);

            return Ok(newUser);
        }
        [HttpPost("password")]
        [Authorize]
        public async Task<ActionResult<UserDetail>> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out int userId))
            {
                return Unauthorized();
            }

            string response = await _appUserService.ChangePasswordAsync(userId, request);

            if (!string.IsNullOrEmpty(response)) return BadRequest(response);

            return Ok();
        }
    }
}
