using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Models;
using SearchEngineHitCounter.Services.Apis;
using Shouldly;

namespace SearchEngineHitCounterTests.Services.Apis
{
    [TestClass]
    public class TwitterApiServiceTest : TestBase
    {
        private const string TwitterApiBaseUrl = "TwitterAPISettings:BaseUrl";

        [TestMethod]
        public async Task TwitterApiService_GetSearchHits_ErrorResponseFromApi_ShouldReturnZeroHits()
        {
            const string expectedSearchText = nameof(expectedSearchText);
            const string expectedBearerToken = nameof(expectedBearerToken);
            const string expectedBaseUrl = "https://api.twitter.com/2/tweets/counts/recent";
            long expectedHits = 0;

            var mockConfigSection = new Mock<IConfigurationSection>();
            mockConfigSection.Setup(x => x.Value).Returns(expectedBaseUrl);

            var restResponse = new Mock<IRestResponse<TweetCountResult>>();
            restResponse
                .Setup(x => x.StatusCode)
                .Returns(HttpStatusCode.Unauthorized);

            using (var mock = GetAutoMock())
            {
                mock.Mock<IConfiguration>()
                    .Setup(x => x.GetSection(TwitterApiBaseUrl))
                    .Returns(mockConfigSection.Object);
                mock.Mock<IBearerTokenService>()
                    .Setup(x => x.GetBearerToken(It.IsAny<BearerTokenAppSetting>()))
                    .ReturnsAsync(expectedBearerToken);
                mock.Mock<IRestClient>()
                    .Setup(x => x.ExecuteAsync<TweetCountResult>(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(restResponse.Object));

                var result = await mock.Create<TwitterApiService>().GetSearchHits(expectedSearchText);
                result.ShouldBe(expectedHits);
            }
        }

        [TestMethod]
        public async Task TwitterApiService_GetSearchHits_SuccessResponseFromApi_ShouldParseHitsAndReturn()
        {
            const string expectedSearchText = nameof(expectedSearchText);
            const string expectedBearerToken = nameof(expectedBearerToken);
            const string expectedBaseUrl = "https://api.twitter.com/2/tweets/counts/recent";
            long expectedHits = 100000000;
            var expectedTwitterCountResult = new TweetCountResult { Meta = new TweetCountResult.MetaData { TotalTweetCount = expectedHits.ToString() } };

            var mockConfigSection = new Mock<IConfigurationSection>();
            mockConfigSection.Setup(x => x.Value).Returns(expectedBaseUrl);

            var restResponse = new Mock<IRestResponse<TweetCountResult>>();
            restResponse
                .Setup(x => x.Data)
                .Returns(expectedTwitterCountResult);
            restResponse
                .Setup(x => x.StatusCode)
                .Returns(HttpStatusCode.OK);

            using (var mock = GetAutoMock())
            {
                mock.Mock<IConfiguration>()
                    .Setup(x => x.GetSection(TwitterApiBaseUrl))
                    .Returns(mockConfigSection.Object);
                mock.Mock<IBearerTokenService>()
                    .Setup(x => x.GetBearerToken(It.IsAny<BearerTokenAppSetting>()))
                    .ReturnsAsync(expectedBearerToken);
                mock.Mock<IRestClient>()
                    .Setup(x => x.ExecuteAsync<TweetCountResult>(It.IsAny<IRestRequest>(), It.IsAny<CancellationToken>()))
                    .Returns(Task.FromResult(restResponse.Object));

                var result = await mock.Create<TwitterApiService>().GetSearchHits(expectedSearchText);
                result.ShouldBe(expectedHits);
            }
        }
    }
}
