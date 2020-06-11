using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Scrapers
{
    class CBCScraper : ICBCScraper
    {
        private static IWebDriver driver;
        private readonly List<String> _searchTerms;
        private IWebsiteScraperShared _websiteScraperShared;

        public CBCScraper(IWebsiteScraperShared websiteScraperShared, List<String> searchTerms)
        {
            _searchTerms = searchTerms;
            _websiteScraperShared = websiteScraperShared;
        }

        public async Task<List<WebsiteData>> getWebsiteData()
        {
            List<WebsiteData> cbcData = new List<WebsiteData>();
            driver = new ChromeDriver();
            List<String> urls = await getUrls();

            foreach (var item in urls)
            {
                driver.Navigate().GoToUrl(item);
                try
                {
                    var headlineElement = driver.FindElement(By.ClassName("detailHeadline"));
                    foreach (var term in _searchTerms)
                    {
                        if (headlineElement.Text.ToLower().Contains(term))
                        {
                            WebsiteData websiteData = new WebsiteData();
                            websiteData.outlet = "CBC News";
                            websiteData.url = item;
                            websiteData.headline = headlineElement.Text;
                            try
                            {
                                websiteData.date = driver.FindElement(By.ClassName("timeStamp")).Text.Substring(8, 12);
                            }
                            catch
                            {
                                Console.WriteLine("No date found");
                                websiteData.date = "";

                            }
                            try
                            {
                                websiteData.body = driver.FindElement(By.ClassName("story")).Text;
                            }
                            catch
                            {
                                Console.WriteLine("No article body found");
                                websiteData.body = "";
                            }

                            cbcData.Add(websiteData);
                        }
                    }
                }
                catch
                {
                    Console.WriteLine("No headline found.");
                }
            }

            return cbcData;

        }

        public async Task<List<String>> getUrls()
        {
            HttpClient client = new HttpClient();
            List<String> urls = new List<String>();

            for (int i = 1; i < 100; i++)
            {
                HttpResponseMessage response = await client.GetAsync($"https://www.cbc.ca/search_api/v1/search?q=coronavirus&sortOrder=relevance&section=news&page={i}&fields=feed");

                List<CBCNewsAPI> content = await response.Content.ReadAsAsync<List<CBCNewsAPI>>();
                foreach (var item in content)
                {
                    var url = $"https:{item.url}";
                    urls.Add(url);
                }
            }

            return urls;
        }
    }
}
