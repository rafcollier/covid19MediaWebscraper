using Data_Scraper_Proj.Models;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace Data_Scraper_Proj.Interfaces
{
    interface IWebsiteScraper
    {
        IReadOnlyCollection<IWebElement> FindElements(By by);
        List<WebsiteData> getWebsiteData(List<string> searchTerms);
    }
}