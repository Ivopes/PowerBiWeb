using PowerBiWeb.Client.Utilities.Http;
using PowerBiWeb.Shared.Datasets;

namespace PowerBiWeb.Client.Utilities.Interfaces
{
    public interface IDatasetService
    {
        Task<HttpResponse<List<DatasetDTO>>> GetAllAsync();
        Task<HttpResponse<DatasetDTO>> GetDatasetDetailAsync(int id);
        Task<HttpResponse<DatasetDTO>>  AddDatasetAsync(DatasetDTO dataset, bool addNew);
        Task<HttpResponse>  DeleteDatasetAsync(int datasetId);
    }
}