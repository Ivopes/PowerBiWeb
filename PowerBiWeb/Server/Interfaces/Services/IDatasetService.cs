using PowerBiWeb.Shared.Datasets;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IDatasetService
    {
        Task<DatasetDTO?> AddDatasetByIdAsync(string datasetId, string name);
        Task<DatasetDTO?> AddExistingDatasetByIdAsync(string datasetId, Guid datasetGuid);
        Task<bool> DeleteByIdAsync(int id);
        Task<List<DatasetDTO>> GetAllAsync();
        Task<DatasetDTO?> GetByIdAsync(int id);
    }
}