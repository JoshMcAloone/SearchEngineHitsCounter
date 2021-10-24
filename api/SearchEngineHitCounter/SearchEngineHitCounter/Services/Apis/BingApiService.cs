using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Extensions;

namespace SearchEngineHitCounter.Services.Apis
{
    public class BingApiService : IBingApiService
    {
        private readonly IWebsiteHtmlScraper websiteHtmlScraper;
        private readonly IConfiguration configuration;

        private const string BingApiBaseUrl = "BingAPISettings:BaseUrl";

        public BingApiService(
            IWebsiteHtmlScraper websiteHtmlScraper,
            IConfiguration configuration)
        {
            this.websiteHtmlScraper = websiteHtmlScraper;
            this.configuration = configuration;
        }

        public async Task<long> GetSearchHits(string searchText)
        {
            var baseUrl = configuration.GetValue<string>(BingApiBaseUrl);

            var resultsInnerHtml = await websiteHtmlScraper.GetElementInnerHtml($"{baseUrl}?q={searchText}", "//span[contains(@class, 'sb_count')]");

            if (string.IsNullOrWhiteSpace(resultsInnerHtml))
            {
                return 0;
            }

            // Remove comma chars
            var resultString = resultsInnerHtml.RemoveNonBreakingSpaces();

            // Take first part of string which is like 'nnnnnnn resultat'
            return long.Parse(resultString.Split(" ")[0]);
        }
    }
}
