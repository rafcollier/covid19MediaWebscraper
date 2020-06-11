using Data_Scraper_Proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Interfaces
{
    interface ICBCScraper
    {
        Task<List<string>> getUrls();
        Task<List<WebsiteData>> getWebsiteData();
    }
}