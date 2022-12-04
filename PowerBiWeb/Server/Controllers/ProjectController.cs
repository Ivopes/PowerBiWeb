using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Shared;

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
        public async Task<ActionResult<List<Project>>> Get()
        {

            return Ok(await _projectService.GetAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Project>> Get(int id)
        {

            return Ok(await _projectService.GetAsync(id));
        }
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Project project)
        {

            return Ok(await _projectService.PostAsync(project));
        }
    }
}
