using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        protected LocalDbContext DbContext { get; set; }


        protected GenericRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ICollection<T> GetAllRecords()
        {
            return DbContext.Set<T>().ToList();
        }

        public async Task<ICollection<T>> GetAllRecordsAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await DbContext.FindAsync<T>(id);
        }

        public IQueryable<T> Query()
        {
            return DbContext.Set<T>();
        }


        public async Task InsertAsync(T entity)
        {
            DbContext.Set<T>().Add(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<T> InsertAndReturnAsync(T entity)
        {
            var ret = DbContext.Set<T>().Add(entity);
            await DbContext.SaveChangesAsync();
            return ret.Entity;
        }

        public async Task UpdateAsync(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int[] ids)
        {
            foreach (var id in ids)
            {
                var entity = DbContext.FindAsync<T>(id);
                DbContext.Remove(entity);
            }
            await DbContext.SaveChangesAsync();
        }

        public IQueryable<T> SqlQuery(string query)
        {
            var result = DbContext.Set<T>().FromSql(query);
            return result;
        }

        public async Task ExecuteCommand(string query)
        {
            await DbContext.Database.ExecuteSqlCommandAsync(query);
        }

        public async Task DeleteAsync(T entity)
        {
            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

    }
}