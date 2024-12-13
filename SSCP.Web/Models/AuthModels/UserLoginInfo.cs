using SSCP.Web.Models.Enums;

namespace SSCP.Web.Models.AuthModels
{
    public class UserLoginInfo
    {
        public AuthEnums Status { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
    }
}
