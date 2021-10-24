using System;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Models.Enums;

namespace SearchEngineHitCounter.Services.Factories
{
    public class SearchEngineApiFactory : ISearchEngineApiFactory
    {
        private readonly ITwitterApiService twitterApiService;
        private readonly IBingApiService bingApiService;

        public SearchEngineApiFactory(
            ITwitterApiService twitterApiService,
            IBingApiService bingApiService)
        {
            this.twitterApiService = twitterApiService;
            this.bingApiService = bingApiService;
        }

        public ISearchEngineApiService GetSearchEngineApi(SearchEngineType searchEngine)
        {
            switch (searchEngine)
            {
                case SearchEngineType.Twitter:
                    return twitterApiService;
                case SearchEngineType.Bing:
                    return bingApiService;
                default:
                    throw new ArgumentException($"Failed to find a search engine api for {searchEngine}");
            }
        }
    }
}
