using Data_Scraper_Proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Interfaces
{
    interface ITwitterScraperUsers
    {
        Task<List<TwitterUser>> PrepareUserInsert(List<string> twitterUserList);
        Task<TwitterData> scrapeTwitterUser(string twitterAccount);
    }
}