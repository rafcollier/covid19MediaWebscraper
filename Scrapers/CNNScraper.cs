using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;

namespace Data_Scraper_Proj.Scrapers
{
    class CNNScraper : ICNNScraper
    {

        private static IWebDriver driver;
        private readonly List<String> _searchTerms;
        private IWebsiteScraperShared _websiteScraperShared;

        public CNNScraper(IWebsiteScraperShared websiteScraperShared, List<String> searchTerms)
        {
            _searchTerms = searchTerms;
            _websiteScraperShared = websiteScraperShared;
        }

        public List<WebsiteData> getWebsiteData()
        {
            List<WebsiteData> cnnData = new List<WebsiteData>();
            driver = new ChromeDriver();

            for(int pageNum = 1; pageNum <100; pageNum++)
            {
                driver.Navigate().GoToUrl($"https://www.cnn.com/search?size=100&q=coronavirus&from={(pageNum - 1) * 100}&page={pageNum}");
                var searchList = _websiteScraperShared.FindElements(driver, By.ClassName("cnn-search__result"));

                foreach (var item in searchList)
                {
                    var headlineElement = item.FindElement(By.ClassName("cnn-search__result-headline"));
                    var dateElement = item.FindElement(By.ClassName("cnn-search__result-publish-date"));
                    var bodyElement = item.FindElement(By.ClassName("cnn-search__result-body"));

                    foreach (var term in _searchTerms)
                    {
                        if (headlineElement.Text.ToLower().Contains(term))
                        {
                            WebsiteData websiteData = new WebsiteData();
                            websiteData.outlet = "CNN";
                            websiteData.url = headlineElement.FindElement(By.TagName("a")).GetAttribute("href");
                            websiteData.headline = headlineElement.Text;
                            websiteData.date = dateElement.Text;
                            websiteData.body = bodyElement.Text;
                            cnnData.Add(websiteData);
                            break;
                        }

                    }
                }
            }
            driver.Close();
            return cnnData;
        }
    }
}
