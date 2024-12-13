using SSCP.Web.Models.AuthModels;

namespace SSCP.Web.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ContributorAuthResponse> CreateUserAsync(ContributorAuth contributor);
        Task<UserLoginInfo> UserLoginAsync(ContributorAuth contributor);
    }
}
