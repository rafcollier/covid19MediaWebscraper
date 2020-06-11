using System.Collections.Generic;

namespace Data_Scraper_Proj.Models
{
    class FoxNewsAPI
    {
        public List<Item> items { get; set; }
    }

    class Item
    {
        public string link { get; set; }
    }
}
