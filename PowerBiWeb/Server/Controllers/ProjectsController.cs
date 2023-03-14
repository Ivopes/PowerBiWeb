using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly IMetricsApiLoaderRepository _metricsApiRepository;
        private readonly IAuthService _authService;
        public ProjectsController(IProjectService projectService, IMetricsApiLoaderRepository metricsApiRepository, IAuthService authService)
        {
            _projectService = projectService;
            _metricsApiRepository = metricsApiRepository;
            _authService = authService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ProjectDTO>>> Get()
        {

            return Ok(await _projectService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> Get(int id)
        {
            var role = await _authService.GetProjectRole(id);
            if (role is null || role > ProjectRoles.Viewer) return Forbid();

            var result = await _projectService.GetAsync(id);

            if (result is null) return NotFound("Project was not found");

            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> Post([FromBody] ProjectDTO project)
        {
            return Ok(await _projectService.PostAsync(project));
        }
        [HttpPost("{projectId}/report")]
        public async Task<ActionResult<ProjectDTO>> AddReport(int projectId, [FromBody] DashboardDTO report)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.AddReportAsync(projectId, report);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpDelete("{projectId}/report/{reportId}")]
        public async Task<ActionResult<ProjectDTO>> RemoveReport(int projectId, Guid reportId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.RemoveReportAsync(projectId, reportId);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpPost("{projectId}/dashboard")]
        public async Task<ActionResult<ProjectDTO>> AddDashboard(int projectId, [FromBody] DashboardDTO dashboard)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.AddDashboardAsync(projectId, dashboard);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpDelete("{projectId}/dashboard/{dashboardId}")]
        public async Task<ActionResult<ProjectDTO>> RemoveDashboard(int projectId, Guid dashboardId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.RemoveDashboardAsync(projectId, dashboardId);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpPost("user")]
        public async Task<ActionResult<string>> AddToUser([FromBody] UserToProjectDTO dto)
        {
            var role = await _authService.GetProjectRole(dto.ProjectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.AddToUserAsync(dto);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpPut("user")]
        public async Task<ActionResult<string>> EditUser([FromBody] UserToProjectDTO dto)
        {
            var role = await _authService.GetProjectRole(dto.ProjectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.EditUserAsync(dto);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpPut("{projectId}")]
        public async Task<ActionResult<string>> EditProject(int projectId, [FromBody] ProjectDTO dto)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.EditProject(projectId, dto);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpDelete("{projectId}/{userId}")]
        public async Task<ActionResult<string>> DeleteUser(int projectId, int userId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.RemoveUserAsync(userId, projectId);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpDelete("{projectId}")]
        public async Task<ActionResult<string>> DeleteProject(int projectId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role != ProjectRoles.Creator) return Forbid();

            var result = await _projectService.RemoveProject(projectId);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
    }
}
