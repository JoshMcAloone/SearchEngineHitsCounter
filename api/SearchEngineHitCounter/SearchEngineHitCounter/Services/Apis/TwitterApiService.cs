using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RestSharp;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Exceptions;
using SearchEngineHitCounter.Extensions;
using SearchEngineHitCounter.Models;

namespace SearchEngineHitCounter.Services.Apis
{
    public class TwitterApiService : ITwitterApiService
    {
        private readonly IBearerTokenService bearerTokenService;
        private readonly IConfiguration configuration;
        private readonly IRestClient restClient;

        private const string TwitterMemCacheKey = "twitterApiBearerToken";
        private const string TwitterApiBaseUrl = "TwitterAPISettings:BaseUrl";
        private const string TwitterApiKey = "TwitterAPISettings:ApiKey";
        private const string TwitterApiSecret = "TwitterAPISettings:ApiSecret";
        private const string TwitterAuthUrl = "TwitterAPISettings:BearerTokenUrl";

        public TwitterApiService(
            IBearerTokenService bearerTokenService,
            IConfiguration configuration,
            IRestClient restClient)
        {
            this.bearerTokenService = bearerTokenService;
            this.configuration = configuration;
            this.restClient = restClient;
        }

        public async Task<long> GetSearchHits(string searchText)
        {
            var baseUrl = configuration.GetValue<string>(TwitterApiBaseUrl);

            restClient.BaseUrl = new Uri(baseUrl);
            var request = new RestRequest(Method.GET);

            request.AddParameter("query", searchText, ParameterType.QueryString);

            var bearerTokenAppSetting = new BearerTokenAppSetting
            {
                AuthUrl = TwitterAuthUrl,
                ApiKey = TwitterApiKey,
                ApiSecret = TwitterApiSecret,
                MemCacheKey = TwitterMemCacheKey
            };
            request.AddHeader("Authorization", $"Bearer {await bearerTokenService.GetBearerToken(bearerTokenAppSetting)}");

            try
            {
                var response = await restClient.ExecuteAsync<TweetCountResult>(request);
                ValidateStatus(response);
                return long.Parse(response.Data.Meta.TotalTweetCount);
            }
            catch (ExternalApiException)
            {
                return 0;
            }
        }

        private void ValidateStatus(IRestResponse response)
        {
            if (!response.StatusCode.IsSuccessStatusCode())
            {
                throw new ExternalApiException($"{response.StatusCode}: {response.ErrorMessage}", "Twitter", response.ErrorException);
            }
        }
    }
}
