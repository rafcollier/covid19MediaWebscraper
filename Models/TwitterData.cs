namespace Data_Scraper_Proj.Models
{
    public class TwitterData
    {
        public string Created_at { get; set; }
        public string Id_str { get; set; }
        public string Full_text { get; set; }
        public int Retweet_count { get; set; }
        public int Favorite_count { get; set; }
        public string Screen_name { get; set; }
        public TwitterUser User { get; set; }
    }
}
