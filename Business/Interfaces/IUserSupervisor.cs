using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Entities;
using Entities.Classes;

namespace Business.Interfaces
{
    public interface IUserSupervisor
    {
        Task<User> CreateUser(User User);
        Task<User> UpdateUser(Guid id, User User);
        Task<User> GetUser(Guid id);
        Task<ICollection<User>> GetUsers();
        Task DeleteUser(Guid id);
        Task<object> AuthenticateUser(string userName, string password);
    }
}
