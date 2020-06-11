using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data_Scraper_Proj.Models;
using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Scrapers;
using MySqlX.XDevAPI.Relational;
using System.Linq;

namespace Data_Scraper_Proj.Data_Access
{
    public class TweetDataSQL : ITweetDataSQL
    {

        private readonly ISqlAccess _sqlAccess;

        public TweetDataSQL(ISqlAccess sqlAccess)
        {
            _sqlAccess = sqlAccess;
        }

        public async Task InsertTwitterUsers(List<TwitterUser> twitterUserList, String tableName)
        {
            foreach (var user in twitterUserList)
            {
                await InsertTwitterUser(user, tableName);
            };
        }

        public async Task InsertTwitterDataAll(List<TwitterData> tweetsAll, String tableName)
        {
            foreach (var tweet in tweetsAll)
            {
                await InsertTwitterData(tweet, tableName);
            };
        }

        public async Task<List<TwitterUser>> GetTwitterUsers()
        {
            string sql = "select * from twitteruser";
            List<TwitterUser> twitterUsers = new List<TwitterUser>();
            twitterUsers = await _sqlAccess.LoadData<TwitterUser, dynamic>(sql, new { });
            return twitterUsers;
        }

        public async Task InsertTwitterUser(TwitterUser twitterUser, String tableName)
        {
            string sql = $@"insert into {tableName} (Id_str, Screen_name, Description, Followers_count)
                           values (@Id_str, @Screen_name, @Description, @Followers_count)";
            await _sqlAccess.SaveData(sql, twitterUser);
        }

        public async Task InsertTwitterData(TwitterData twitterData, String tableName)
        {
            string sql = $@"insert into {tableName} (Id_str, Created_at, Full_text, Retweet_Count, Favorite_count, Screen_name)
                           values (@Id_str, @Created_at, @Full_text, @Retweet_Count, @Favorite_count, @Screen_name)";
            await _sqlAccess.SaveData(sql, twitterData);
        }
    }
}
