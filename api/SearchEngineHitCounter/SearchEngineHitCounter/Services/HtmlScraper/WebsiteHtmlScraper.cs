using System.Threading.Tasks;
using HtmlAgilityPack;
using SearchEngineHitCounter.Contracts;

namespace SearchEngineHitCounter.Services.HtmlScraper
{
    public class WebsiteHtmlScraper : IWebsiteHtmlScraper
    {
        public async Task<string> GetElementInnerHtml(string url, string xpath)
        {
            var htmlWeb = new HtmlWeb();
            var document = await htmlWeb.LoadFromWebAsync(url);
            return document.DocumentNode.SelectSingleNode(xpath).InnerHtml;
        }
    }
}
