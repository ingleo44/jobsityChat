using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;

namespace Entities.Repositories
{
    public interface IGenericRepository<T>
    {
        ICollection<T> GetAllRecords();

        Task<ICollection<T>> GetAllRecordsAsync();

        Task<T> GetAsync(int id);

        IQueryable<T> Query();

        Task InsertAsync(T entity);

        Task<T> InsertAndReturnAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(int[] ids);

        IQueryable<T> SqlQuery(string query);

    }
}

