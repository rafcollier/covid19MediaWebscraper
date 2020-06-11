using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Models;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Scrapers
{
    class FoxNewsScraper : IFoxNewsScraper
    {

        private static IWebDriver driver;
        private readonly List<String> _searchTerms;
        private IWebsiteScraperShared _websiteScraperShared;

        public FoxNewsScraper(IWebsiteScraperShared websiteScraperShared, List<String> searchTerms)
        {
            _searchTerms = searchTerms;
            _websiteScraperShared = websiteScraperShared;
        }

        public async Task<List<WebsiteData>> getWebsiteData()
        {
            List<WebsiteData> FoxNewsData = new List<WebsiteData>();
            driver = new ChromeDriver();
            var urls = await getUrls();

            foreach(var item in urls)
            {
                driver.Navigate().GoToUrl(item.url);
                var headlineElement = driver.FindElement(By.ClassName("headline"));

                foreach (var term in _searchTerms)
                {
                    if (headlineElement.Text.ToLower().Contains(term))
                    {
                        WebsiteData websiteData = new WebsiteData();
                        websiteData.outlet = "Fox News";
                        websiteData.url = item.url;
                        websiteData.headline = headlineElement.Text;
                        websiteData.date = item.date;
                        websiteData.body = driver.FindElement(By.ClassName("article-body")).Text;
                        FoxNewsData.Add(websiteData);
                    }
                }
            }

            return FoxNewsData;
        }

        public async Task<List<WebsiteData>> getUrls()
        {
            List<WebsiteData> urlAndDate = new List<WebsiteData>(); 
            HttpClient client = new HttpClient();

            for (int i = 1; i < 100; i++)
            {
                var date = DateTime.Today.AddDays(1-i).ToString("yyyyMMdd");
                var dateFormatted = DateTime.Today.AddDays(1-i).ToString("MMM dd, yyyy");
                HttpResponseMessage response = await client.GetAsync($"https://api.foxnews.com/search/web?q=coronavirus+more:pagemap:metatags-pagetype:article+more:pagemap:metatags-dc.type:Text.Article&siteSearch=foxnews.com&siteSearchFilter=i&sort=date:r:{date}:{date}");

                var content = response.Content.ReadAsStringAsync().Result;
                var apiObjects = JsonConvert.DeserializeObject<FoxNewsAPI>(content);
                foreach(var item in apiObjects.items)
                {
                    var foxNewsObject = new WebsiteData();
                    foxNewsObject.url = item.link;
                    foxNewsObject.date = dateFormatted;
                    urlAndDate.Add(foxNewsObject);
                }
            }

            return urlAndDate;
        }
    }
}
