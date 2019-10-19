using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Business.Interfaces;
using Entities.Classes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Business.Classes
{
    public class Authentication : IAuthentication
    {
        private readonly IDistributedCache _cache;
        private readonly IOptions<JwtOptions> _jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Authentication( IDistributedCache cache, IOptions<JwtOptions> jwtOptions, IHttpContextAccessor httpContextAccessor)
        {

            _cache = cache;
            _jwtOptions = jwtOptions;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<JwtToken> GenerateToken(string username, DateTime? clientTime)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expDate = DateTime.Now.AddMinutes(_jwtOptions.Value.ExpiryMinutes);

            if (clientTime != null)
            {
                expDate = clientTime.Value.AddMinutes(_jwtOptions.Value.ExpiryMinutes);
            }

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Value.Issuer,
                audience: _jwtOptions.Value.Issuer,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtOptions.Value.ExpiryMinutes),
                signingCredentials: credentials);
            var response = new JwtSecurityTokenHandler().WriteToken(token);
            return new JwtToken { Token = response, ExpDate = expDate };
        }



        public async Task<JwtToken> GenerateToken(string username)
        {
            return await GenerateToken(username, null);
        }

        public async Task<bool> IsCurrentActiveToken()
                 => await IsActiveAsync(GetCurrentAsync());


        public async Task<bool> IsActiveAsync(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());

        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(' ').Last();
        }

        public async Task DeactivateAsync(string token)
            => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(_jwtOptions.Value.ExpiryMinutes)
                });




        private static string GetKey(string token)
            => $"tokens:{token}:deactivated";


    }
}