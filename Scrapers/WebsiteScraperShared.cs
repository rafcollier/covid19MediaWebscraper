using Data_Scraper_Proj.Interfaces;
using OpenQA.Selenium;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Data_Scraper_Proj.Scrapers
{
    class WebsiteScraperShared : IWebsiteScraperShared
    {
        public IReadOnlyCollection<IWebElement> FindElements(IWebDriver driver, By by)
        {
            Stopwatch w = Stopwatch.StartNew();
            while (w.ElapsedMilliseconds < 10 * 1000)
            {
                var elements = driver.FindElements(by);
                if (elements.Count > 0)
                {
                    return elements;
                }
                Thread.Sleep(10);
            }
            return null;
        }
    }
}
