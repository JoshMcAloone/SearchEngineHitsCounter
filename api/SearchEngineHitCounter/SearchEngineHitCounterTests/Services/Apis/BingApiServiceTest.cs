using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Services.Apis;
using Shouldly;

namespace SearchEngineHitCounterTests.Services
{
    [TestClass]
    public class BingApiServiceTest : TestBase
    {
        private const string BingApiBaseUrl = "BingAPISettings:BaseUrl";

        [TestMethod]
        public async Task BingApiService_GetSearchHits_InnerHtmlIsNull_ShouldReturnZeroHits()
        {
            const string expectedSearchText = nameof(expectedSearchText);
            const string expectedBaseUrl = "https://www.bing.com/search";
            var expectedWebsiteUrl = $"{expectedBaseUrl}?q={expectedSearchText}";
            const string expectedXPath = "//span[contains(@class, 'sb_count')]";
            long expectedHits = 0;

            var mockConfigSection =  new Mock<IConfigurationSection>();
            mockConfigSection.Setup(x => x.Value).Returns(expectedBaseUrl);

            using (var mock = GetAutoMock())
            {
                mock.Mock<IConfiguration>()
                    .Setup(x => x.GetSection(BingApiBaseUrl))
                    .Returns(mockConfigSection.Object);
                mock.Mock<IWebsiteHtmlScraper>()
                    .Setup(x => x.GetElementInnerHtml(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(null as string);

                var result = await mock.Create<BingApiService>().GetSearchHits(expectedSearchText);
                result.ShouldBe(expectedHits);

                mock.Mock<IWebsiteHtmlScraper>().Verify(x => x.GetElementInnerHtml(expectedWebsiteUrl, expectedXPath), Times.Once);
            }
        }

        [TestMethod]
        public async Task BingApiService_GetSearchHits_InnerHtmlFound_ShouldParseAndReturnHits()
        {
            const string expectedSearchText = nameof(expectedSearchText);
            const string expectedBaseUrl = "https://www.bing.com/search";
            var expectedWebsiteUrl = $"{expectedBaseUrl}?q={expectedSearchText}";
            const string expectedXPath = "//span[contains(@class, 'sb_count')]";
            const string expectedInnerHtml = "51&#160;100&#160;000 resultat";
            long expectedHits = 51100000;

            var mockConfigSection = new Mock<IConfigurationSection>();
            mockConfigSection.Setup(x => x.Value).Returns(expectedBaseUrl);

            using (var mock = GetAutoMock())
            {
                mock.Mock<IConfiguration>()
                    .Setup(x => x.GetSection(BingApiBaseUrl))
                    .Returns(mockConfigSection.Object);
                mock.Mock<IWebsiteHtmlScraper>()
                    .Setup(x => x.GetElementInnerHtml(It.IsAny<string>(), It.IsAny<string>()))
                    .ReturnsAsync(expectedInnerHtml);

                var result = await mock.Create<BingApiService>().GetSearchHits(expectedSearchText);
                result.ShouldBe(expectedHits);

                mock.Mock<IWebsiteHtmlScraper>().Verify(x => x.GetElementInnerHtml(expectedWebsiteUrl, expectedXPath), Times.Once);
            }
        }
    }
}
