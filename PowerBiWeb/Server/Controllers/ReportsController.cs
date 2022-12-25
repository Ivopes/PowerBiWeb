using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<EmbedParams>> Get()
        {

            var result = await _reportService.GetAsync();

            return Ok(result);

        }
    }
}
