using Data_Scraper_Proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Interfaces

{
    interface IFoxNewsScraper
    {
        Task<List<WebsiteData>> getWebsiteData();
        Task<List<WebsiteData>> getUrls();
    }
}