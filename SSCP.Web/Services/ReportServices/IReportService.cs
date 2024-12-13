using SSCP.Web.Models.RepoModels;
using SSCP.Web.Models.ReportModels;

namespace SSCP.Web.Services.ReportServices
{
    public interface IReportService
    {
        Task<RepositoryInsightsDto> GetRepositoryInsightsAsync(RepositoryInsightsDto repository);
        Task<List<GithubRepo>> ListRepositoriesAsync();
        Task<bool> StoreReposAsync(List<GithubRepo> repos);
        Task<ComprehensiveAnomalyReport> GetAnomalyReportAsync(RepositoryInsightsDto repository);
    }
}
