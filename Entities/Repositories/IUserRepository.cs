using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Entities.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(Guid id, User user);
        Task<User> GetUser(Guid id);
        Task<ICollection<User>> GetUsers();
        Task DeleteUser(Guid id);
    }
}