using Microsoft.EntityFrameworkCore;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Models.Contexts;
using PowerBiWeb.Server.Models.Entities;

namespace PowerBiWeb.Server.Repositories
{
    public class DatasetRepository : IDatasetRepository
    {
        private readonly PowerBiContext _dbContext;

        public DatasetRepository(PowerBiContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> DeleteByIdAsync(int id)
        {
            var entity = await _dbContext.Datasets.FindAsync(id);

            if (entity is null) return false;

            _dbContext.Datasets.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<List<PBIDataset>> GetAllAsync()
        {
            return await _dbContext.Datasets.ToListAsync();
        }
        public async Task<PBIDataset?> GetAsync(string metricFilesId)
        {
            return await _dbContext.Datasets.SingleOrDefaultAsync(d => d.MetricFilesId == metricFilesId);
        }
        public async Task<PBIDataset?> GetByIdAsync(int id)
        {
            return await _dbContext.Datasets.FindAsync(id);
        }
        public async Task<PBIDataset> PostAsync(PBIDataset d)
        {
            await _dbContext.Datasets.AddAsync(d);

            _dbContext.SaveChanges();

            return d;
        }
    }
}
