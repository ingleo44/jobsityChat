using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;
using Entities;
using Entities.Classes;
using Entities.Repositories;

namespace Business.Classes
{
    public class UserSupervisor : IUserSupervisor
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthentication _authentication;
             

        public UserSupervisor(IUserRepository userRepository, IAuthentication authentication)
        {
            _userRepository = userRepository;
            _authentication = authentication;
        }

        public async Task<User> CreateUser(User user)
        {
            user.Password = EncryptPassword(user.Password);
            return await _userRepository.CreateUser(user);
        }
        public async Task<User> UpdateUser(Guid id, User user) => await _userRepository.UpdateUser(id, user);
        public async Task<User> GetUser(Guid id) => await _userRepository.GetUser(id);
        public async Task<ICollection<User>> GetUsers() => await _userRepository.GetUsers();
        public async Task DeleteUser(Guid id) => await _userRepository.DeleteUser(id);

        public async Task<object> AuthenticateUser(string userName, string password)
        {
            var encryptedPassword = EncryptPassword(password);
            var user = GetUsers().Result
                .FirstOrDefault(q => q.UserName == userName && q.Password == encryptedPassword);
            if (user != null)
            {

                var token = await _authentication.GenerateToken(userName);
                return new
                {
                    user, token
                };

            }

            return null;

        }

        private string EncryptPassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            var newPassword = System.Text.Encoding.ASCII.GetString(data);
            return newPassword;
        }
    }
}
