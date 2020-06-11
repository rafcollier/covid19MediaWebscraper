using System.Threading.Tasks;

namespace Data_Scraper_Proj.Interfaces
{
    interface ISharedSQL
    {
        Task ClearTable(string tableName);
    }
}