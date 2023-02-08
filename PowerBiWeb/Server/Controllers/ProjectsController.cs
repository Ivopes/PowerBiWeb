using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            if (await _authService.GetProjectRole(id) > ProjectRoles.Viewer) return Forbid();

            var result = await _projectService.GetAsync(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> Post([FromBody] ProjectDTO project)
        {
            //TODO: Pridat moznost nahrat uz hotove reporty z PowerBI a vypnout automaticke nahrani z MetrikAPI
            return Ok(await _projectService.PostAsync(project));
        }
        [HttpPost("{projectId}/report")]
        public async Task<ActionResult<ProjectDTO>> AddReport(int projectId, [FromBody] EmbedContentDTO report)
        {
            if (await _authService.GetProjectRole(projectId) > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.AddReportAsync(projectId, report);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpDelete("{projectId}/report/{reportId}")]
        public async Task<ActionResult<ProjectDTO>> RemoveReport(int projectId, Guid reportId)
        {
            if (await _authService.GetProjectRole(projectId) > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.RemoveReportAsync(projectId, reportId);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpPost("{projectId}/dashboard")]
        public async Task<ActionResult<ProjectDTO>> AddDashboard(int projectId, [FromBody] EmbedContentDTO dashboard)
        {
            if (await _authService.GetProjectRole(projectId) > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.AddDashboardAsync(projectId, dashboard);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpDelete("{projectId}/dashboard/{dashboardId}")]
        public async Task<ActionResult<ProjectDTO>> RemoveDashboard(int projectId, Guid dashboardId)
        {
            if (await _authService.GetProjectRole(projectId) > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.RemoveDashboardAsync(projectId, dashboardId);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpPost("user")]
        public async Task<ActionResult<string>> AddToUser([FromBody] UserToProjectDTO dto)
        {
            if (await _authService.GetProjectRole(dto.ProjectId) > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.AddToUserAsync(dto);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpPut("user")]
        public async Task<ActionResult<string>> EditUser([FromBody] UserToProjectDTO dto)
        {
            if (await _authService.GetProjectRole(dto.ProjectId) > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.EditUserAsync(dto);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpPut("{projectId}")]
        public async Task<ActionResult<string>> EditProject(int projectId, [FromBody] ProjectDTO dto)
        {
            if (await _authService.GetProjectRole(projectId) > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.EditProject(projectId, dto);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpDelete("{projectId}/{userId}")]
        public async Task<ActionResult<string>> DeleteUser(int projectId, int userId)
        {
            if (await _authService.GetProjectRole(projectId) > ProjectRoles.Editor) return Forbid();

            var result = await _projectService.RemoveUserAsync(userId, projectId);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpDelete("{projectId}")]
        public async Task<ActionResult<string>> DeleteProject(int projectId)
        {
            if ((await _projectService.GetProjectRole(projectId)) != ProjectRoles.Creator) return Forbid();

            var result = await _projectService.RemoveProject(projectId);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
    }
}
