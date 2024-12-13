using Blazored.SessionStorage;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Ocsp;
using Plugin.DoubleEncryption;
using SSCP.Web.Models.Constants;
using SSCP.Web.Models.SharedModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json;

namespace SSCP.Web.Services.SharedServices
{
    public class APIHandler : IAPIHandler
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISessionStorageService _sessionStorage;
        private readonly AesBcCrypto _aesGcm;

        public APIHandler(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor,
            ISessionStorageService sessionStorage, AesBcCrypto aesBcCrypto)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _sessionStorage = sessionStorage;
            _aesGcm = aesBcCrypto;
        }

        public async Task<TReturn> GetAsync<TReturn>(string url, string baseUrl, Dictionary<string, string> headers = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            SetClientAsync(client, baseUrl);
            SetHeaders(headers, client);
            var response = await client.GetAsync(url);
            return await GetResponseContentAsync<TReturn>(response, baseUrl, url);
        }

        public async Task<TReturn> PostAsync<TReturn>(string json, string url, string baseUrl, Dictionary<string, string> headers = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            await SetClientAsync(client, baseUrl);
            SetHeaders(headers, client);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsJsonAsync(url, stringContent);
            return await GetNormalResponseContentAsync<TReturn>(response, baseUrl, url);
        }

        public async Task<TReturn> PostAsync<TParam,TReturn>(TParam json, string url, string baseUrl, Dictionary<string, string> headers = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            SetClientAsync(client, baseUrl);
            SetHeaders(headers, client);
            var stringContent = new StringContent(JsonConvert.SerializeObject(json), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, stringContent);
            return await GetResponseContentAsync<TReturn>(response, baseUrl, url);
        }

        public async Task<TReturn> EncryptedPostAsync<TReturn>(string json, string url, string baseUrl, Dictionary<string, string> headers = null)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            var request = EncryptRequestObject(json);
            await SetClientAsync(client, baseUrl);
            SetHeaders(headers, client);
            var stringContent = new StringContent(System.Text.Json.JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, stringContent);
            return await GetResponseContentAsync<TReturn>(response, baseUrl, url);
        }


        private async Task SetClientAsync(HttpClient client, string baseUrl)
        {
            client.BaseAddress = new Uri(baseUrl);
            await SetAuthTokenAsync(client);


        }

        private void SetHeaders(Dictionary<string, string> headers, HttpClient client)
        {
            if (headers is not null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

            }
        }

        private async Task<TReturn> GetResponseContentAsync<TReturn>(HttpResponseMessage response, string baseUrl, string url)
        {
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    var safeResponse = JsonConvert.DeserializeObject<SafeResponseDto>(await response.Content.ReadAsStringAsync());
                    return Decrypt<TReturn>(safeResponse.Response, safeResponse.ERK);
                }
                return default;
            }
            string value = await response.Content.ReadAsStringAsync();
            throw new Exception($"API call failed: {baseUrl}{url} with error - {value}");
        }

        private async Task<TReturn> GetNormalResponseContentAsync<TReturn>(HttpResponseMessage response, string baseUrl, string url)
        {
            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                {
                    var safeResponse = JsonConvert.DeserializeObject<TReturn>(await response.Content.ReadAsStringAsync());
                    return safeResponse;
                }
                return default;
            }
            string value = await response.Content.ReadAsStringAsync();
            throw new Exception($"API call failed: {baseUrl}{url} with error - {value}");
        }

        private async Task SetAuthTokenAsync(HttpClient client)
        {
            var authToken = await _sessionStorage.GetItemAsync<string>("AuTon");
            if (!string.IsNullOrEmpty(authToken))
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            }
        }

        private string EncryptRequest(string request)
        {
            var randomKey = Guid.NewGuid().ToString("N");
            var encryptedRandomKey = _aesGcm.Encrypt(randomKey, Encoding.UTF8.GetBytes(AuthConstants.AuthKey));
            var encryptedRequest = _aesGcm.Encrypt(randomKey, Encoding.UTF8.GetBytes(randomKey));
            SafeRequestDto safeRequestDto = new SafeRequestDto();
            safeRequestDto.Request = encryptedRequest;
            safeRequestDto.ERK = encryptedRandomKey;
            return JsonConvert.SerializeObject(safeRequestDto);


        }

        private SafeRequestDto EncryptRequestObject(string request)
        {
            var randomKey = Guid.NewGuid().ToString("N");
            var encryptedRandomKey = _aesGcm.Encrypt(randomKey, Encoding.UTF8.GetBytes(AuthConstants.AuthKey));
            var encryptedRequest = _aesGcm.Encrypt(request, Encoding.UTF8.GetBytes(randomKey));
            SafeRequestDto safeRequestDto = new SafeRequestDto();
            safeRequestDto.Request = encryptedRequest;
            safeRequestDto.ERK = encryptedRandomKey;
            return safeRequestDto;


        }

        private T Decrypt<T>(string encryptedRequest, string encryptedRandomKey)
        {
            var randomKey = _aesGcm.Decrypt(encryptedRandomKey, Encoding.UTF8.GetBytes(AuthConstants.AuthKey));
            if (!string.IsNullOrEmpty(encryptedRequest))
            {
                var request = JsonConvert.DeserializeObject<T>(_aesGcm.Decrypt(encryptedRequest, Encoding.UTF8.GetBytes(randomKey)));
                return request;
            }

            return default;

        }
    }
}
