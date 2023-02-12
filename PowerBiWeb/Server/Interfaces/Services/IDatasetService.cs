using PowerBiWeb.Shared.Datasets;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IDatasetService
    {
        Task<List<DatasetDTO>> GetAllAsync();
        Task<DatasetDTO?> GetByIdAsync(int id);
    }
}