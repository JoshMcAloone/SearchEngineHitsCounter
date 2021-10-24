using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Models.Enums;
using SearchEngineHitCounter.Services.Factories;
using Shouldly;

namespace SearchEngineHitCounterTests.Factories
{
    [TestClass]
    public class SearchEngineApiFactoryTest : TestBase
    {
        [TestMethod]
        public void SearchEngineApiFactory_GetSearchEngineApi_ReturnsBingApiService()
        {
            TestSearchEngineApiFactory<IBingApiService>(SearchEngineType.Bing);
        }

        [TestMethod]
        public void SearchEngineApiFactory_GetSearchEngineApi_ReturnsTwitterApiService()
        {
            TestSearchEngineApiFactory<ITwitterApiService>(SearchEngineType.Twitter);
        }

        [TestMethod]
        public void SearchEngineApiFactory_GetSearchEngineApi_ThrowsIfNoMatching()
        {
            using (var mock = GetAutoMock())
            {
                Should.Throw<ArgumentException>(() => mock.Create<SearchEngineApiFactory>().GetSearchEngineApi(SearchEngineType.Google));
            }
        }

        private void TestSearchEngineApiFactory<T>(SearchEngineType searchEngineType)
        {
            using (var mock = GetAutoMock())
            {
                var loader = mock.Create<SearchEngineApiFactory>().GetSearchEngineApi(searchEngineType);
                loader.ShouldBeAssignableTo<T>();
                loader.ShouldNotBeNull();
            }
        }
    }
}
