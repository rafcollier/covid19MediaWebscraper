using Autofac;
using Data_Scraper_Proj.Config;
using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IContainer = Autofac.IContainer;

namespace Data_Scraper_Proj
{
    class Program
    {
        private static string _sqlConnectionString = System.IO.File.ReadAllText(@"C:\Projects\Data_Scraper\SqlConnectionString.txt");
        private static List<String> _accountsCanadaList = System.IO.File.ReadAllText(@"C:\Projects\Data_Scraper\newsOutletsCanada.txt").Split(',').ToList();
        private static List<String> _accountsUSList = System.IO.File.ReadAllText(@"C:\Projects\Data_Scraper\newsOutletsUS.txt").Split(',').ToList();
        private static List<String> _searchTerms = System.IO.File.ReadAllText(@"C:\Projects\Data_Scraper\searchTerms.txt").Split(',').ToList();
        private static string _twitterToken = System.IO.File.ReadAllText(@"C:\Projects\Data_Scraper\TwitterToken.txt");
        private static IContainer _container;

        static async Task Main(string[] args)
        {
            _container = ContainerConfig.Configure(
                _twitterToken,
                _searchTerms,
                _sqlConnectionString
            );

            using (var scope = _container.BeginLifetimeScope())
            {
                ////////////////////////////////////////////////////////////////
                // Shared Components 
                ////////////////////////////////////////////////////////////////

                var sharedSQL = scope.Resolve<ISharedSQL>();
                var tweetDataSQL = scope.Resolve<ITweetDataSQL>();
                var websiteDataSQL = scope.Resolve<IWebsiteDataSQL>();

                ////////////////////////////////////////////////////////////////
                // Clear database tables
                ////////////////////////////////////////////////////////////////

                await sharedSQL.ClearTable("twitterusercanada");
                await sharedSQL.ClearTable("twitteruserus");
                await sharedSQL.ClearTable("twitterdata");
                await sharedSQL.ClearTable("websitedata");

                ////////////////////////////////////////////////////////////////
                // Get Twitter Users and Add to Database
                ////////////////////////////////////////////////////////////////

                var twitterScraperUsers = scope.Resolve<ITwitterScraperUsers>();
                List<TwitterUser> twitterUsersCanada = await twitterScraperUsers.PrepareUserInsert(_accountsCanadaList);
                await tweetDataSQL.InsertTwitterUsers(twitterUsersCanada, "twitterusercanada");
                List<TwitterUser> twitterUsersUS = await twitterScraperUsers.PrepareUserInsert(_accountsUSList);
                await tweetDataSQL.InsertTwitterUsers(twitterUsersUS, "twitteruserus");

                ////////////////////////////////////////////////////////////////
                // Get MAX Tweets (3200) From Each User 
                ////////////////////////////////////////////////////////////////

                var twitterScraperTweets = scope.Resolve<ITwitterScraperTweets>();
                List<TwitterData> twitterDataAllCanada = await twitterScraperTweets.PrepareTweetInsert(_accountsCanadaList);
                await tweetDataSQL.InsertTwitterDataAll(twitterDataAllCanada, "twitterdata");
                List<TwitterData> twitterDataAllUS = await twitterScraperTweets.PrepareTweetInsert(_accountsUSList);
                await tweetDataSQL.InsertTwitterDataAll(twitterDataAllUS, "twitterdata");

                ////////////////////////////////////////////////////////////////
                // Scrape Coronavirus news articles from CNN 
                ////////////////////////////////////////////////////////////////

                var cnnScraper = scope.Resolve<ICNNScraper>();
                List<WebsiteData> cnnDataList = cnnScraper.getWebsiteData();
                await websiteDataSQL.PrepareWebsiteDataInsert(cnnDataList, "websitedata");

                ////////////////////////////////////////////////////////////////
                // Scrape Coronavirus news articles from Fox News 
                ////////////////////////////////////////////////////////////////

                var foxNewsScraper = scope.Resolve<IFoxNewsScraper>();
                List<WebsiteData> foxNewsDataList = await foxNewsScraper.getWebsiteData();
                await websiteDataSQL.PrepareWebsiteDataInsert(foxNewsDataList, "websitedata");

                ////////////////////////////////////////////////////////////////
                // Scrape Coronavirus news articles from CBC News 
                ////////////////////////////////////////////////////////////////

                var CBCScraper = scope.Resolve<ICBCScraper>();
                List<WebsiteData> CBCDataList = await CBCScraper.getWebsiteData();
                await websiteDataSQL.PrepareWebsiteDataInsert(CBCDataList, "websitedata");

            }
        }
    }
}
