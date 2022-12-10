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
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
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
        [HttpPost("adduser")]
        public async Task<ActionResult<string>> AddToUser([FromBody] AddUserToObjectDTO dto)
        {
            if (await _projectService.IsMinEditor(dto.ProjectId)) return Forbid("You dont have permision");

            var result = await _projectService.AddToUser(dto);

            if (string.IsNullOrEmpty(result)) return Ok(result);

            return BadRequest(result);
        }
    }
}
