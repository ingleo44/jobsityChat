using System;
using System.Threading.Tasks;
using Business.Interfaces;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserSupervisor _userSupervisor;

        public UserController(IUserSupervisor userSupervisor)
        {
            _userSupervisor = userSupervisor;
        }

        // GET: api/User
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _userSupervisor.GetUsers();
            return new ObjectResult(result);
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        [Authorize]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await _userSupervisor.GetUser(id);
            return  new ObjectResult(result);
        }

        // POST: api/User
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserViewModel user)
        {
            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                UserName = user.UserName
            };
            var result = await _userSupervisor.CreateUser(newUser);
            return new ObjectResult(result);

        }

        // PUT: api/User/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] UserViewModel user)
        {
            var newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                UserName = user.UserName,
                Id = user.Id
            };
            var result = await _userSupervisor.UpdateUser(id,newUser);
            return new ObjectResult(result);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _userSupervisor.DeleteUser(id);
            return new ObjectResult(new { ok = true });

        }
    }

    public class UserViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public Guid Id { get; set; }
    }
}
