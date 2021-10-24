using SearchEngineHitCounter.Models.Enums;

namespace SearchEngineHitCounter.Contracts
{
    public interface ISearchEngineApiFactory
    {
        ISearchEngineApiService GetSearchEngineApi(SearchEngineType searchEngine);
    }
}
