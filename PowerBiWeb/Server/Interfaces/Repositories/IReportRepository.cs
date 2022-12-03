using PowerBiWeb.Shared;

namespace PowerBiWeb.Server.Interfaces.Repositories
{
    public interface IReportRepository
    {
        Task<EmbedParams> GetAsync();
    }
}