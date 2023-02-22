using PowerBiWeb.Shared.Datasets;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IDatasetService
    {
        Task<DatasetDTO?> AddDatasetByIdAsync(string datasetId);
        Task<DatasetDTO?> AddExistingDatasetByIdAsync(DatasetDTO dataset);
        Task<bool> DeleteByIdAsync(int id);
        Task<List<DatasetDTO>> GetAllAsync();
        Task<DatasetDTO?> GetByIdAsync(int id);
    }
}