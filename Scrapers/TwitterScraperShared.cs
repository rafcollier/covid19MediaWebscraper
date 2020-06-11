using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace Data_Scraper_Proj.Scrapers
{
    public class TwitterScraperShared : ITwitterScraperShared
    {
        private HttpClient _client;

        public TwitterScraperShared(HttpClient client)
        {
            _client = client;
        }
        public async Task<List<TwitterData>> GetTwitterDataAsync(String count, String accountName, String max_id)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = "api.twitter.com";
            uriBuilder.Path = "1.1/statuses/user_timeline.json";
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["screen_name"] = accountName;
            query["count"] = count;
            query["tweet_mode"] = "extended";
            if (max_id != null) query["max_id"] = max_id;
            uriBuilder.Query = query.ToString();
            String url = uriBuilder.ToString(); 

            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url))
            {
                Version = HttpVersion.Version10
            };

            var response = await _client.SendAsync(request);
            var data = response.Content.ReadAsAsync<List<TwitterData>>().Result;
            return data;
        }
    }
}
