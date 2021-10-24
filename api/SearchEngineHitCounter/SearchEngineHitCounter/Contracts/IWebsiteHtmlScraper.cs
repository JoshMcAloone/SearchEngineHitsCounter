using System.Threading.Tasks;

namespace SearchEngineHitCounter.Contracts
{
    public interface IWebsiteHtmlScraper
    {
        Task<string> GetElementInnerHtml(string url, string xpath);
    }
}
