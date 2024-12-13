using Newtonsoft.Json;
using SSCP.Web.Models.AuthModels;
using SSCP.Web.Services.SharedServices;

namespace SSCP.Web.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IAPIHandler _apiHandler;
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public AuthService(IAPIHandler aPIHandler, IConfiguration configuration)
        {
            _apiHandler = aPIHandler;
            _configuration = configuration;
            _apiUrl = _configuration.GetValue<string>("APIUrl:SecurityWebhookUrl");
        }

        public async Task<ContributorAuthResponse> CreateUserAsync(ContributorAuth contributor)
        {
            var request = JsonConvert.SerializeObject(contributor);
            var response = await _apiHandler.EncryptedPostAsync<ContributorAuthResponse>(request, "api/auth/signup", _apiUrl);
            return response;
        }

        public async Task<UserLoginInfo> UserLoginAsync(ContributorAuth contributor)
        {
            var request = JsonConvert.SerializeObject(contributor);
            var response = await _apiHandler.EncryptedPostAsync<UserLoginInfo>(request, "api/auth/login", _apiUrl);
            return response;
        }

    }
}
