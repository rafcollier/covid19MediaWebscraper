using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Scrapers
{
    public class TwitterScraperTweets : ITwitterScraperTweets
    {
        private ITwitterScraperShared _twitterScraperShared;
        private List<String> _searchTerms;
        private List<TwitterData> _userTweetsAll;

        public TwitterScraperTweets(ITwitterScraperShared twitterScraperShared, List<String> searchTerms)
        {
            _twitterScraperShared = twitterScraperShared;
            _searchTerms = searchTerms;
            _userTweetsAll = new List<TwitterData>();
        }

        public async Task<List<TwitterData>> PrepareTweetInsert(List<String> twitterUserList)
        {
            foreach (var user in twitterUserList)
            {
                string max_id = null;
                await RepeatTwitterApiCall(user, max_id);
            }
            return _userTweetsAll;
        }

        public async Task RepeatTwitterApiCall(String user, String max_id)
        {
            List<TwitterData> twitterData = await scrapeTwitterData(user, max_id);

            //Twitter API will return only one tweet (the max_id tweet) once reach limit (3200) of tweets available
            if (twitterData != null && twitterData.Count > 1)
            {
                //The max_id tweet is fetched on previous call so don't include.
                twitterData = twitterData.Where(x => x.Id_str != max_id).ToList();
                max_id = twitterData.LastOrDefault().Id_str;

                foreach (var tweet in twitterData)
                {
                    foreach (var item in _searchTerms)
                    {
                        if (tweet.Full_text.ToLower().Contains(item))
                        {
                            TwitterData newTwitterData = new TwitterData
                            {
                                Id_str = tweet.Id_str,
                                Created_at = tweet.Created_at,
                                Full_text = tweet.Full_text,
                                Retweet_count = tweet.Retweet_count,
                                Favorite_count = tweet.Favorite_count,
                                Screen_name = tweet.User.Screen_name
                            };
                            _userTweetsAll.Add(newTwitterData);
                            break;
                        }
                    }
                }
                await Task.Delay(5000);
                await RepeatTwitterApiCall(user, max_id);
            }
            else
            {
                Console.WriteLine("Done.");
            }
        }

        public async Task<List<TwitterData>> scrapeTwitterData(String twitterAccount, String max_id)
        {
            List<TwitterData> twitterList = await _twitterScraperShared.GetTwitterDataAsync("200", twitterAccount, max_id);
            return twitterList;
        }

    }

}
