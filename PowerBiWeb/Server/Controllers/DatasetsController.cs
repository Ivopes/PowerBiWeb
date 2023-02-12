using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Shared.Datasets;

namespace PowerBiWeb.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatasetsController : ControllerBase
    {
        private readonly IDatasetService _datasetService;

        public DatasetsController(IDatasetService datasetService)
        {
            _datasetService = datasetService;
        }

        [HttpGet]
        public async Task<ActionResult<List<DatasetDTO>>> GetAll()
        {
            return Ok(await _datasetService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<DatasetDTO>> GetById(int id)
        {
            var result = await _datasetService.GetByIdAsync(id);

            if (result is null) return NotFound("Dataset was not found");

            return Ok(result);
        }
    }
}
