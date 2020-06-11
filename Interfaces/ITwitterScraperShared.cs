using Data_Scraper_Proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Interfaces
{
    public interface ITwitterScraperShared
    {
        Task<List<TwitterData>> GetTwitterDataAsync(string count, string accountName, string max_id);
    }
}