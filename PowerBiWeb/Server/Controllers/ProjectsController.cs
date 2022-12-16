using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpGet]
        public async Task<ActionResult<List<ProjectDTO>>> Get()
        {
            
            return Ok(await _projectService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> Get(int id)
        {
            var result = await _projectService.GetAsync(id);
            return Ok(result);
        }
        [HttpPost]
        public async Task<ActionResult<ProjectDTO>> Post([FromBody] ProjectDTO project)
        {
            return Ok(await _projectService.PostAsync(project));
        }
        [HttpPost("user")]
        public async Task<ActionResult<string>> AddToUser([FromBody] UserToProjectDTO dto)
        {
            if (!await _projectService.IsMinEditor(dto.ProjectId)) return Forbid();

            var result = await _projectService.AddToUserAsync(dto);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpPut("user")]
        public async Task<ActionResult<string>> EditUser([FromBody] UserToProjectDTO dto)
        {
            if (!await _projectService.IsMinEditor(dto.ProjectId)) return Forbid();

            var result = await _projectService.EditUserAsync(dto);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
        [HttpDelete("{projectId}/{userId}")]
        public async Task<ActionResult<string>> DeleteUser(int projectId, int userId)
        {
            if (!await _projectService.IsMinEditor(projectId)) return Forbid();

            var result = await _projectService.RemoveUserAsync(userId, projectId);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
    }
}
