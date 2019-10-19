using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(LocalDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<User> CreateUser(User user)
        {
            return await InsertAndReturnAsync(user);
        }

        public async Task<User> UpdateUser(Guid id, User user)
        {
            await UpdateAsync(user);
            return await Query().Where(q => q.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUser(Guid id)
        {
            return await Query().Where(q => q.Id == id).FirstOrDefaultAsync();
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await Query().ToListAsync();
        }

        public async Task DeleteUser(Guid id)
        {
            var entity = await GetUser(id);
            await DeleteAsync(entity);
        }
    }
}
