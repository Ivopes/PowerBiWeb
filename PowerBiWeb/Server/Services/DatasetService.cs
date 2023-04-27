using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.PowerBI.Api.Models;
using NuGet.Packaging;
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

        public async Task<DatasetDTO?> AddDatasetByIdAsync(string datasetId)
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
            MetricData? data = await _metricsApiLoaderRepository.GetMetricTotal(datasetId);

            if (data is null)
            {
                throw new MessageException { ExcptMessage = "Dataset could not been created - cant load data from metric server" };
            }

            // Nacist metric data z metric serveru
            MetricData? inc = await _metricsApiLoaderRepository.GetMetricIncrement(datasetId);

            if (inc is not null)
            {
                data.Rows.AddRange(inc.Rows);
                data.Name = inc.Name;
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
        public async Task<DatasetDTO?> AddExistingDatasetByIdAsync(DatasetDTO dataset)
        {
            PBIDataset newDataset;

            // nacist definici a pripravit ji k vytvoreni v DB
            MetricDefinition? definition = await _metricsApiLoaderRepository.GetMetricDefinition(dataset.MetricFilesId);

            if (definition is null)
            {
                throw new MessageException { ExcptMessage = "Dataset could not been created. Check the dataset ID" };
            }

            newDataset = new PBIDataset
            {
                Name = definition.Name,
                PowerBiId = dataset.PowerBiId,
                MetricFilesId = dataset.MetricFilesId,
                ColumnNames = definition.ColumnNames,
                ColumnTypes = definition.ColumnTypes,
                Measures = definition.Measures,
                MeasureDefinitions = definition.MeasureDefinitions
            };

            await _datasetRepository.PostAsync(newDataset);

            return newDataset.ToDTO();
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

        public async Task UpdateAllAsync()
        {
            var datasets = await _datasetRepository.GetAllAsync();
            foreach (var dataset in datasets)
            {
                try
                {
                    var result = await _metricsApiLoaderRepository.GetMetricIncrement(dataset.MetricFilesId);

                    if (result is not null)
                    {
                        if (result.Name == dataset.LastUpdateName) continue;
                        
                        await _metricsSaverRepository.AddRowsToDataset(dataset, result);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Could not auto-update metrics for dataset: {0}, with id: {1}", dataset.MetricFilesId, dataset.Id);
                }
            }
        }
        public async Task<string> UpdateByIdAsync(int datasetId)
        {
            var dataset = await _datasetRepository.GetByIdAsync(datasetId);
            if (dataset is null) return "Dataset not found";
            try
            {
                var result = await _metricsApiLoaderRepository.GetMetricIncrement(dataset.MetricFilesId);

                if (result is not null)
                {
                    if (result.Name != dataset.LastUpdateName)
                    {
                        bool success = await _metricsSaverRepository.AddRowsToDataset(dataset, result);
                        if (!success)
                        {
                            return "Dataset could not been updated";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not update metrics for dataset: {0}, with id: {1}", dataset.MetricFilesId, dataset.Id);
            }

            return string.Empty;
        }
    }
}
