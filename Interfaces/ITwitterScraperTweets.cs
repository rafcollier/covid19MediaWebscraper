using Data_Scraper_Proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Interfaces
{
    public interface ITwitterScraperTweets
    {
        Task<List<TwitterData>> PrepareTweetInsert(List<string> twitterUserList);
        Task RepeatTwitterApiCall(string user, string max_id);
        Task<List<TwitterData>> scrapeTwitterData(string twitterAccount, string max_id);
    }
}