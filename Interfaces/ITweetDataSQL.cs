using Data_Scraper_Proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Interfaces
{
    public interface ITweetDataSQL
    {
        Task<List<TwitterUser>> GetTwitterUsers();
        Task InsertTwitterData(TwitterData twitterData, string tableName);
        Task InsertTwitterDataAll(List<TwitterData> tweetsAll, string tableName);
        Task InsertTwitterUser(TwitterUser twitterUser, string tableName);
        Task InsertTwitterUsers(List<TwitterUser> twitterUserList, string tableName);
    }
}