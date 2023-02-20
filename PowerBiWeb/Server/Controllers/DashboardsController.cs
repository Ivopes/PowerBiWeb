﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        private readonly IAuthService _authService;
        public DashboardsController(IDashboardService dashboardService, IAuthService authService)
        {
            _dashboardService = dashboardService;
            _authService = authService;
        }
        [HttpGet("{projectId:int}/{reportId:Guid}")]
        public async Task<ActionResult<EmbedContentDTO>> GetByIdAsync(int projectId, Guid reportId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Viewer) return Forbid();

            var embed = await _dashboardService.GetByIdAsync(projectId, reportId);

            if (embed is null) return NotFound("Dashboard was not found");

            return Ok(embed);
        }
        [HttpGet("{projectId}/update")]
        public async Task<ActionResult<string>> UpdateDashboardsAsync(int projectId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Viewer) return Forbid();

            var result = await _dashboardService.UpdateDashboardsAsync(projectId);

            return Ok(result);
        }
    }
}
