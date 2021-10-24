using Autofac;
using RestSharp;
using SearchEngineHitCounter.Contracts;
using SearchEngineHitCounter.Services.Apis;
using SearchEngineHitCounter.Services.Authorisation;
using SearchEngineHitCounter.Services.Factories;
using SearchEngineHitCounter.Services.HtmlScraper;
using SearchEngineHitCounter.Services.SearchEngine;

namespace SearchEngineHitCounter.Autofac
{
    public class ServiceModules : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterType<SearchEngineApiFactory>().As<ISearchEngineApiFactory>();
            builder.RegisterType<TwitterApiService>().As<ITwitterApiService>();
            builder.RegisterType<BingApiService>().As<IBingApiService>();
            builder.RegisterType<BearerTokenService>().As<IBearerTokenService>();
            builder.RegisterType<SearchEngineService>().As<ISearchEngineService>();
            builder.RegisterType<WebsiteHtmlScraper>().As<IWebsiteHtmlScraper>();
            builder.RegisterType<RestClient>().As<IRestClient>();
        }
    }
}
