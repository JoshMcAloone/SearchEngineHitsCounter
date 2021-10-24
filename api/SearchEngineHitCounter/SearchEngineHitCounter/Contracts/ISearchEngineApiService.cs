using System.Threading.Tasks;

namespace SearchEngineHitCounter.Contracts
{
    public interface ISearchEngineApiService
    {
        Task<long> GetSearchHits(string searchText);
    }
}
