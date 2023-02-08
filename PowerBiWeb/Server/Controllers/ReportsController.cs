using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;

namespace PowerBiWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IAuthService _authService;
        public ReportsController(IReportService reportService, IAuthService authService)
        {
            _reportService = reportService;
            _authService = authService;
        }

        [HttpGet("{projectId:int}")]
        public ActionResult<EmbedParams> GetAsync(int projectId)
        {
            //var result = await _reportService.GetAsync(projectId);
            return Ok();
        }
        [HttpGet("{projectId:int}/{reportId:Guid}")]
        public async Task<ActionResult<EmbedContentDTO>> GetByIdAsync(int projectId, Guid reportId)
        {
            if (await _authService.GetProjectRole(projectId) > ProjectRoles.Viewer) return Forbid();

            var embed = await _reportService.GetByIdAsync(projectId, reportId);

            return Ok(embed);
        }
        [HttpGet("{projectId}/update")]
        public async Task<ActionResult<string>> UpdateReportsAsync(int projectId)
        {
            if (await _authService.GetProjectRole(projectId) > ProjectRoles.Viewer) return Forbid();

            var result = await _reportService.UpdateReportsAsync(projectId);

            return Ok(result);

        }
    }
}
