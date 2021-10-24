using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RestSharp;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Models;

namespace SearchEngineHitCounter.Services.Authorisation
{
    public class BearerTokenService : IBearerTokenService
    {
        private readonly IConfiguration configuration;
        private readonly IMemoryCache memoryCache;
        private readonly IRestClient restClient;

        public BearerTokenService(
            IConfiguration configuration,
            IMemoryCache memoryCache,
            IRestClient restClient)
        {
            this.configuration = configuration;
            this.memoryCache = memoryCache;
            this.restClient = restClient;
        }

        public async Task<string> GetBearerToken(BearerTokenAppSetting bearerTokenAppSetting)
        {
            if (memoryCache.TryGetValue(bearerTokenAppSetting.MemCacheKey, out var res))
            {
                return res.ToString();
            }

            var response = await GetToken(bearerTokenAppSetting);

            // Twitter token does not expire, but could set expiration here
            memoryCache.Set(bearerTokenAppSetting.MemCacheKey, response.Data.AccessToken, DateTimeOffset.MaxValue);

            return memoryCache.Get(bearerTokenAppSetting.MemCacheKey)?.ToString();
        }

        private async Task<IRestResponse<BearerToken>> GetToken(BearerTokenAppSetting bearerTokenAppSetting)
        {
            var request = GetRequest(bearerTokenAppSetting);

            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=client_credentials", ParameterType.RequestBody);

            return await restClient.ExecuteAsync<BearerToken>(request);
        }

        private RestRequest GetRequest(BearerTokenAppSetting bearerTokenAppSetting)
        {
            var authUrl = configuration.GetValue<string>(bearerTokenAppSetting.AuthUrl);
            var apiKey = configuration.GetValue<string>(bearerTokenAppSetting.ApiKey);
            var apiSecret = configuration.GetValue<string>(bearerTokenAppSetting.ApiSecret);

            restClient.BaseUrl = new Uri(authUrl);
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", CreateAuthorisationHeader(apiKey, apiSecret));
            return request;
        }

        private string CreateAuthorisationHeader(string apiKey, string apiSecret)
        {
            return "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes($"{apiKey}:{apiSecret}"));
        }
    }
}
