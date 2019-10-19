using System.Net;
using System.Threading.Tasks;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;

namespace ChatHub.Middleware
{
    public class TokenManagerMiddleWare : IMiddleware
    {
        private readonly IAuthentication _tokenManager;

        public TokenManagerMiddleWare(IAuthentication tokenManager)
        {
            _tokenManager = tokenManager;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (await _tokenManager.IsCurrentActiveToken())
            {
                await next(context);

                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}