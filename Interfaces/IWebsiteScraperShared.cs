using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;

namespace Data_Scraper_Proj.Interfaces
{
    interface IWebsiteScraperShared
    {
        IReadOnlyCollection<IWebElement> FindElements(IWebDriver driver, By by);
    }
}