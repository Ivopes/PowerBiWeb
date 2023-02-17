using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api.Models;
using PowerBiWeb.Server.Interfaces.Repositories;
using PowerBiWeb.Server.Interfaces.Services;
using PowerBiWeb.Server.Models.Entities;
using PowerBiWeb.Server.Models.Metrics;
using PowerBiWeb.Server.Utilities.Exceptions;
using PowerBiWeb.Server.Utilities.Extentions;
using PowerBiWeb.Shared.Datasets;

namespace PowerBiWeb.Server.Services
{
    public class DatasetService : IDatasetService
    {
        private readonly IDatasetRepository _datasetRepository;
        private readonly IMetricsApiLoaderRepository _metricsApiLoaderRepository;
        private readonly IMetricsContentRepository _metricsSaverRepository;
        private readonly ILogger<DatasetService> _logger;

        public DatasetService(IDatasetRepository datasetRepository, IMetricsApiLoaderRepository metricsApiLoaderRepository, IMetricsContentRepository metricsSaverRepository, ILogger<DatasetService> logger)
        {
            _datasetRepository = datasetRepository;
            _metricsApiLoaderRepository = metricsApiLoaderRepository;
            _metricsSaverRepository = metricsSaverRepository;
            _logger = logger;
        }

        public async Task<DatasetDTO?> AddDatasetByIdAsync(string datasetId, string name)
        {
            PBIDataset dataset;

            // nejdrive nacist definici a pripravit ji k vytvoreni v DB
            MetricDefinition? definition = await _metricsApiLoaderRepository.GetMetricDefinition(datasetId);

            if (definition is null)
            {
                throw new MessageException { ExcptMessage = "Dataset could not been created. Check the dataset ID" };
            }

            dataset = new PBIDataset
            {
                Name = definition.Name,
                MetricFilesId = datasetId,
                ColumnNames = definition.ColumnNames,
                ColumnTypes = definition.ColumnTypes,
                Measures = definition.Measures,
                MeasureDefinitions = definition.MeasureDefinitions
            };

            // Nacist metric data z metric serveru
            MetricData? data = await _metricsApiLoaderRepository.GetMetricTotalNew(datasetId);

            if (data is null)
            {
                throw new MessageException { ExcptMessage = "Dataset could not been created - cant load data from metric server" };
            }

            // Vytvorit dataset v power BI
            Dataset? createdD = await _metricsSaverRepository.CreateDatasetFromDefinition(definition);
            if (createdD is null)
            {
                throw new MessageException { ExcptMessage = "Dataset could not been created" };
            }

            // Vytvorit pbiDataset v DB
            dataset.PowerBiId = Guid.Parse(createdD.Id);

            await _datasetRepository.PostAsync(dataset);

            //Upload metric data do powerBI

            bool success = await _metricsSaverRepository.AddRowsToDataset(dataset, data);

            if (!success)
            {
                throw new MessageException { ExcptMessage = "Rows could not been pushed" };
            }

            return dataset.ToDTO();
        }
        public async Task<DatasetDTO?> AddExistingDatasetByIdAsync(string datasetId, Guid datasetGuid)
        {
            PBIDataset dataset;

            // nacist definici a pripravit ji k vytvoreni v DB
            MetricDefinition? definition = await _metricsApiLoaderRepository.GetMetricDefinition(datasetId);

            if (definition is null)
            {
                throw new MessageException { ExcptMessage = "Dataset could not been created. Check the dataset ID" };
            }

            dataset = new PBIDataset
            {
                Name = definition.Name,
                PowerBiId = datasetGuid,
                MetricFilesId = datasetId,
                ColumnNames = definition.ColumnNames,
                ColumnTypes = definition.ColumnTypes,
                Measures = definition.Measures,
                MeasureDefinitions = definition.MeasureDefinitions
            };

            await _datasetRepository.PostAsync(dataset);

            return dataset.ToDTO();
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            return await _datasetRepository.DeleteByIdAsync(id);
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
