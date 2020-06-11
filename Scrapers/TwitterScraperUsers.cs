using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Scrapers
{
    public class TwitterScraperUsers : ITwitterScraperUsers
    {

        private ITwitterScraperShared _twitterScraperShared;

        public TwitterScraperUsers(ITwitterScraperShared twitterScraperShared)
        {
            _twitterScraperShared = twitterScraperShared;
        }

        public async Task<List<TwitterUser>> PrepareUserInsert(List<String> twitterUserList)
        {
            List<TwitterUser> twitterUserData = new List<TwitterUser>();
            foreach (var user in twitterUserList)
            {
                TwitterData twitterData = await scrapeTwitterUser(user);
                TwitterUser newTwitterUser = new TwitterUser
                {
                    Id_str = twitterData.User.Id_str,
                    Screen_name = twitterData.User.Screen_name,
                    Description = twitterData.User.Description,
                    Followers_count = twitterData.User.Followers_count
                };
                twitterUserData.Add(newTwitterUser);
            };
            return twitterUserData;
        }

        public async Task<TwitterData> scrapeTwitterUser(String twitterAccount)
        {
            List<TwitterData> twitterList = await _twitterScraperShared.GetTwitterDataAsync("1", twitterAccount, null);
            return twitterList[0];
        }

    }

}
