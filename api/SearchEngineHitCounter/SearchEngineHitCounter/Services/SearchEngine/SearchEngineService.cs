using System.Collections.Generic;
using System.Threading.Tasks;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Models;
using SearchEngineHitCounter.Models.Enums;

namespace SearchEngineHitCounter.Services.SearchEngine
{
    public class SearchEngineService : ISearchEngineService
    {
        private readonly ISearchEngineApiFactory searchEngineApiFactory;

        public SearchEngineService(ISearchEngineApiFactory searchEngineApiFactory)
        {
            this.searchEngineApiFactory = searchEngineApiFactory;
        }

        public async Task<IEnumerable<SearchResult>> PerformSearches(string searchText)
        {
            long twitterHits = 0;
            long bingHits = 0;
            foreach (var word in searchText.Split(' '))
            {
                twitterHits += await searchEngineApiFactory.GetSearchEngineApi(SearchEngineType.Twitter).GetSearchHits(word);
                bingHits += await searchEngineApiFactory.GetSearchEngineApi(SearchEngineType.Bing).GetSearchHits(word);
            }

            return new List<SearchResult> {
                new SearchResult { SearchEngineName = "Twitter (last 7 days)", NumberOfHits = twitterHits },
                new SearchResult { SearchEngineName = "Bing", NumberOfHits = bingHits }
            };
        }
    }
}
