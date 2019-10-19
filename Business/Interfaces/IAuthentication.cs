using System;
using System.Threading.Tasks;
using Entities.Classes;

namespace Business.Interfaces
{
    public interface IAuthentication
    {
   
        Task<JwtToken> GenerateToken(string username);
        Task<JwtToken> GenerateToken(string username, DateTime? clientTime);
        Task<bool> IsCurrentActiveToken();
        Task DeactivateCurrentAsync();
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);

    }
}