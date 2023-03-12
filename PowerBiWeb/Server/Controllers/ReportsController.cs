using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared;
using PowerBiWeb.Shared.Project;
using System.Data;
using System.IO;

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
        [HttpGet("{projectId:int}/{reportId:Guid}")]
        public async Task<ActionResult<DashboardDTO>> GetByIdAsync(int projectId, Guid reportId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Viewer) return Forbid();

            var embed = await _reportService.GetByIdAsync(projectId, reportId);

            if (embed is null) return NotFound("Report was not found");

            return Ok(embed);
        }
        [HttpGet("{projectId}/update")]
        public async Task<ActionResult<string>> UpdateReportsAsync(int projectId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Viewer) return Forbid();

            var result = await _reportService.UpdateReportsAsync(projectId);

            return Ok(result);
        }
        [HttpPost("clone/{projectId}/{reportId}")]
        public async Task<ActionResult<string>> CloneReportAsync(int projectId, Guid reportId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _reportService.CloneReportAsync(projectId, reportId);

            return Ok(result);
        }
        [HttpPost("rebind/{projectId}/{reportId}/{datasetId}")]
        public async Task<ActionResult<string>> RebindReportAsync(int projectId, Guid reportId, Guid datasetId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _reportService.RebindReportAsync(projectId, reportId, datasetId);

            if (string.IsNullOrEmpty(result)) return Ok();
            
            return BadRequest(result);
        }
        [HttpPut]
        public async Task<ActionResult<string>> UpdateReportSettingsAsync([FromBody] ReportDTO report)
        {
            var projectId = report.Projects[0].Id;
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Editor) return Forbid();

            var result = await _reportService.UpdateReportSettingsAsync(report);

            if (string.IsNullOrEmpty(result)) return Ok();
            
            return BadRequest(result);
        }
        [HttpGet("export/{projectId}/{reportId}")]
        public async Task<ActionResult<string>> ExportReportAsync(int projectId, Guid reportId)
        {
            var role = await _authService.GetProjectRole(projectId);
            if (role is null || role > ProjectRoles.Viewer) return Forbid();

            var result = await _reportService.ExportReportAsync(projectId, reportId);

            if (result is null) return NotFound();

            var s = new MemoryStream();

            await result.CopyToAsync(s);

            s.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(s, "application/octet-stream");
        }
    }
}
