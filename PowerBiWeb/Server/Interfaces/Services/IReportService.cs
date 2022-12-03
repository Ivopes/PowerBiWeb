using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Services
{
    public interface IReportService
    {
        Task<EmbedParams> GetAsync();
    }
}