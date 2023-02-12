using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Utilities.Extentions;
using PowerBiWeb.Shared.Datasets;

namespace PowerBiWeb.Server.Services
{
    public class DatasetService : IDatasetService
    {
        private readonly IDatasetRepository _datasetRepository;
        public DatasetService(IDatasetRepository datasetRepository)
        {
            _datasetRepository = datasetRepository;
        }

        public async Task<List<DatasetDTO>> GetAllAsync()
        {
            var datasets = await _datasetRepository.GetAllAsync();

            var dtos = new List<DatasetDTO>();

            foreach (var d in datasets)
            {
                dtos.Add(d.ToDTO());
            }


            return dtos;
        }

        public async Task<DatasetDTO?> GetByIdAsync(int id)
        {
            PBIDataset? dataset =  await _datasetRepository.GetByIdAsync(id);

            if (dataset is null) return null;

            var dto = dataset.ToDTO();

            return dto;
        }
    }
}
