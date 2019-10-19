using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IUserSupervisor _userSupervisor;

        public AuthenticationController(IUserSupervisor userSupervisor)
        {
            _userSupervisor = userSupervisor;
        }


        // POST: api/Authentication
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AuthenticationViewModel credentials)
        {
            var result = await _userSupervisor.AuthenticateUser(credentials.UserName, credentials.Password);
            return result == null ? new ObjectResult(new {ok=false}) : new ObjectResult(new { ok = true, data = result });
        }


    }

    public class AuthenticationViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
