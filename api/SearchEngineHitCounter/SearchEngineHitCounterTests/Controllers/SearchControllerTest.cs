using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Controllers;
using Shouldly;

namespace SearchEngineHitCounterTests.Controllers
{
    [TestClass]
    public class SearchControllerTest : TestBase
    {
        [DataTestMethod]
        [DataRow(null)]
        [DataRow(" ")]
        public async Task SearchController_PerformSearch_InvalidSearchText_ShouldReturnBadRequest(string searchText)
        {
            using (var mock = GetAutoMock())
            {
                var response = await mock.Create<SearchController>().PerformSearch(searchText);
                response.ShouldBeAssignableTo<BadRequestObjectResult>("The action should have returned a bad request result");
            }
        }

        [TestMethod]
        public async Task SearchController_PerformSearch_ShouldReturnCorrectly()
        {
            const string expectedSearchText = nameof(expectedSearchText);

            using (var mock = GetAutoMock())
            {
                mock.Mock<ISearchEngineApiService>()
                    .Setup(x => x.GetSearchHits(It.IsAny<string>()))
                    .ReturnsAsync(1000000);

                var response = await mock.Create<SearchController>().PerformSearch(expectedSearchText);

                response.ShouldBeAssignableTo<OkObjectResult>("The action did not return OK along with data");

                mock.Mock<ISearchEngineService>().Verify(x => x.PerformSearches(expectedSearchText), Times.Once);
            }
        }
    }
}
