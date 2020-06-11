using Dapper;
using Data_Scraper_Proj.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Data_Access
{
    public class SQLAccess : ISqlAccess 
    {
        private string _connectionString;

        public SQLAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var data = await connection.QueryAsync<T>(sql, parameters);
                return data.ToList();
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                var data = await connection.ExecuteAsync(sql, parameters);
            }
        }
    }
}
