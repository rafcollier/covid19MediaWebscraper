using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Autofac;
using Data_Scraper_Proj.Scrapers;
using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Data_Access;

namespace Data_Scraper_Proj.Config
{
    public static class ContainerConfig
    {
        public static IContainer Configure(
            String twitterToken,
            List<String> searchTerms,
            String sqlConnectionString
        )
        {
            var builder = new ContainerBuilder();

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", twitterToken);
            builder.Register(c => new TwitterScraperShared(client))
                .As<ITwitterScraperShared>()
                .InstancePerLifetimeScope();

            builder.Register(c => new TwitterScraperUsers(c.Resolve<ITwitterScraperShared>()))
                .As<ITwitterScraperUsers>()
                .InstancePerLifetimeScope();

            builder.Register(c => new TwitterScraperTweets(c.Resolve<ITwitterScraperShared>(), searchTerms))
                .As<ITwitterScraperTweets>()
                .InstancePerLifetimeScope();

            builder.RegisterType<WebsiteScraperShared>()
                .As<IWebsiteScraperShared>()
                .InstancePerLifetimeScope();

            builder.Register(c => new CNNScraper(c.Resolve<IWebsiteScraperShared>(), searchTerms))
                .As<ICNNScraper>()
                .InstancePerLifetimeScope();

            builder.Register(c => new FoxNewsScraper(c.Resolve<IWebsiteScraperShared>(), searchTerms))
                .As<IFoxNewsScraper>()
                .InstancePerLifetimeScope();

            builder.Register(c => new CBCScraper(c.Resolve<IWebsiteScraperShared>(), searchTerms))
                .As<ICBCScraper>()
                .InstancePerLifetimeScope();

            builder.Register(c => new SQLAccess(sqlConnectionString))
                .As<ISqlAccess>()
                .InstancePerLifetimeScope();

            builder.Register(c => new SharedSQL(c.Resolve<ISqlAccess>()))
                .As<ISharedSQL>()
                .InstancePerLifetimeScope();

            builder.Register(c => new WebsiteDataSQL(c.Resolve<ISqlAccess>()))
                .As<IWebsiteDataSQL>()
                .InstancePerLifetimeScope();

            builder.Register(c => new TweetDataSQL(c.Resolve<ISqlAccess>()))
                .As<ITweetDataSQL>()
                .InstancePerLifetimeScope();

            return builder.Build();
        }
    }
}
