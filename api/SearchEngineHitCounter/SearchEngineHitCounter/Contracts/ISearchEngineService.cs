using System.Collections.Generic;
using System.Threading.Tasks;
using SearchEngineHitCounter.Models;

namespace SearchEngineHitCounter.Contracts
{
    public interface ISearchEngineService
    {
        abstract Task<IEnumerable<SearchResult>> PerformSearches(string searchText);
    }
}
