using Data_Scraper_Proj.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data_Scraper_Proj.Interfaces
{
    public interface ISqlAccess
    {
        Task<List<T>> LoadData<T, U>(string sql, U parameters);
        Task SaveData<T>(string sql, T parameters);
    }
}