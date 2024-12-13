namespace SSCP.Web.Services.SharedServices
{
    public interface IAPIHandler
    {
        Task<TReturn> PostAsync<TReturn>(string json, string url, string baseUrl, Dictionary<string, string> headers = null);
        Task<TReturn> PostAsync<TParam, TReturn>(TParam json, string url, string baseUrl, Dictionary<string, string> headers = null);
        Task<TReturn> EncryptedPostAsync<TReturn>(string json, string url, string baseUrl, Dictionary<string, string> headers = null);
    }
}
