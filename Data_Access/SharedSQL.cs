using Data_Scraper_Proj.Interfaces;
using System;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Data_Access
{
    class SharedSQL : ISharedSQL
    {
        private readonly ISqlAccess _sqlAccess;

        public SharedSQL(ISqlAccess sqlAccess)
        {
            _sqlAccess = sqlAccess;
        }

        public async Task ClearTable(String tableName)
        {
            string sql = $@"delete from {tableName}";
            await _sqlAccess.SaveData(sql, new { });
        }


    }
}
