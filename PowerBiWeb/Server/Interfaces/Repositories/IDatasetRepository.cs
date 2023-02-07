using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IDatasetRepository
    {
        Task<PBIDataset?> GetAsync(string metricFilesId);
        Task<PBIDataset> PostAsync(PBIDataset d);
    }
}