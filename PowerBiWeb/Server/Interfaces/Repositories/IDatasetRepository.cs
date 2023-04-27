using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Shared.Datasets;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IDatasetRepository
    {
        Task<bool> DeleteByIdAsync(int id);
        Task<List<PBIDataset>> GetAllAsync();
        Task<PBIDataset?> GetAsync(string metricFilesId);
        Task<PBIDataset?> GetByIdAsync(int id);
        Task<PBIDataset> PostAsync(PBIDataset d);
    }
}