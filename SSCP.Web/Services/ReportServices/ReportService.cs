using Newtonsoft.Json;
using SSCP.Web.Models.RepoModels;
using SSCP.Web.Models.ReportModels;
using SSCP.Web.Services.SharedServices;

namespace SSCP.Web.Services.ReportServices
{
    public class ReportService : IReportService
    {
        private readonly IAPIHandler _apiHandler;
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public ReportService(IAPIHandler aPIHandler, IConfiguration configuration)
        {
            _apiHandler = aPIHandler;
            _configuration = configuration;
            _apiUrl = _configuration.GetValue<string>("APIUrl:SecurityWebhookUrl");
        }

        public async Task<RepositoryInsightsDto> GetRepositoryInsightsAsync(RepositoryInsightsDto repository)
        {

            var request = JsonConvert.SerializeObject(repository);
            var response = await _apiHandler.EncryptedPostAsync<RepositoryInsightsDto>(request, "api/reports/insights", _apiUrl);
            return response;
        }

        public async Task<ComprehensiveAnomalyReport> GetAnomalyReportAsync(RepositoryInsightsDto repository)
        {

            var request = JsonConvert.SerializeObject(repository);
            var response = await _apiHandler.EncryptedPostAsync<ComprehensiveAnomalyReport>(request, "api/reports/anomalyreport", _apiUrl);
            return response;
        }

        public async Task<List<GithubRepo>> ListRepositoriesAsync()
        {
            var response = await _apiHandler.PostAsync<List<GithubRepo>>("", "api/repos/getuserrepos", _apiUrl);
            return response;
        }

        public async Task<bool> StoreReposAsync(List<GithubRepo> repos)
        {
            var request = JsonConvert.SerializeObject(repos);
            var response = await _apiHandler.EncryptedPostAsync<bool>(request, "api/repos/store", _apiUrl);
            return response;
        }
    }
}
