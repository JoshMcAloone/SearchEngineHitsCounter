using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Models;

namespace SearchEngineHitCounter.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchEngineService searchEngineService;

        public SearchController(ISearchEngineService searchEngineService)
        {
            this.searchEngineService = searchEngineService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SearchResult>))]
        public async Task<IActionResult> PerformSearch(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                return BadRequest("No search text was given");
            }

            return Ok(await searchEngineService.PerformSearches(searchText));
        }
        
    }
}
