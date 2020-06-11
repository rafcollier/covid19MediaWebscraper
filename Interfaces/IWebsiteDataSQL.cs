using Data_Scraper_Proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Interfaces
{
    interface IWebsiteDataSQL
    {
        Task InsertWebsiteData(WebsiteData websiteData);
        Task PrepareWebsiteDataInsert(List<WebsiteData> websiteDataList, string tableName);
    }
}