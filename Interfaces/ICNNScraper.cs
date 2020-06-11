using Data_Scraper_Proj.Models;
using System.Collections.Generic;

namespace Data_Scraper_Proj.Interfaces
{
    interface ICNNScraper
    {
        List<WebsiteData> getWebsiteData();
    }
}