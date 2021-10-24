using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Models.Enums;
using SearchEngineHitCounter.Services.SearchEngine;
using Shouldly;

namespace SearchEngineHitCounterTests.Services
{
    [TestClass]
    public class SearchEngineServiceTest : TestBase
    {
        [DataTestMethod]
        [DataRow("test")]
        [DataRow("another test")]
        [DataRow("a b c d e f g h")]
        public async Task SearchEngineService_PerformSearches_CallsCorrectly(string searchText)
        {
            var splitSearchText = searchText.Split(" ");
            var expectedNumberOfSearches = splitSearchText.Length;

            long expectedTwitterHits = 1000000;
            long expectedBingHits = 2000000;

            using (var mock = GetAutoMock())
            {
                mock.Mock<ISearchEngineApiFactory>()
                    .Setup(x => x.GetSearchEngineApi(SearchEngineType.Bing))
                    .Returns(mock.Mock<IBingApiService>().Object);
                mock.Mock<ISearchEngineApiFactory>()
                    .Setup(x => x.GetSearchEngineApi(SearchEngineType.Twitter))
                    .Returns(mock.Mock<ITwitterApiService>().Object);
                mock.Mock<IBingApiService>()
                    .Setup(x => x.GetSearchHits(It.IsAny<string>()))
                    .ReturnsAsync(expectedBingHits);
                mock.Mock<ITwitterApiService>()
                    .Setup(x => x.GetSearchHits(It.IsAny<string>()))
                    .ReturnsAsync(expectedTwitterHits);

                var results = await mock.Create<SearchEngineService>().PerformSearches(searchText);

                var twitterResult = results.Where(x => x.SearchEngineName.Equals("Twitter (last 7 days)")).ShouldHaveSingleItem();
                twitterResult.NumberOfHits.ShouldBe(expectedTwitterHits * expectedNumberOfSearches);

                var bingResult = results.Where(x => x.SearchEngineName.Equals("Bing")).ShouldHaveSingleItem();
                bingResult.NumberOfHits.ShouldBe(expectedBingHits * expectedNumberOfSearches);

                foreach (var word in splitSearchText)
                {
                    mock.Mock<IBingApiService>().Verify(x => x.GetSearchHits(word), Times.Once);
                    mock.Mock<ITwitterApiService>().Verify(x => x.GetSearchHits(word), Times.Once);
                }
            }
        }
    }
}
