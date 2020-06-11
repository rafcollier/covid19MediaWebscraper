using Data_Scraper_Proj.Interfaces;
using Data_Scraper_Proj.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Data_Access
{
    class WebsiteDataSQL : IWebsiteDataSQL
    {
        private readonly ISqlAccess _sqlAccess;


        public WebsiteDataSQL(ISqlAccess sqlAccess)
        {
            _sqlAccess = sqlAccess;
        }

        public async Task PrepareWebsiteDataInsert(List<WebsiteData> websiteDataList, String tableName)
        {
            foreach (var webpage in websiteDataList)
            {
                await InsertWebsiteData(webpage);
            }
        }

        public async Task InsertWebsiteData(WebsiteData websiteData)
        {
            string sql = @"insert into websitedata (url, outlet, headline, date, body)
                         values (@url, @outlet, @headline, @date, @body)";
            await _sqlAccess.SaveData(sql, websiteData);
        }
    }


}
