using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Classes;
using Entities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace jobsityChat.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        IHubContext<ChatHub, ITypedHubClient> _chatHubContext;

        public ChatController(IHubContext<ChatHub, ITypedHubClient> chatHubContext)
        {
            _chatHubContext = chatHubContext;
        }

        // GET: api/Chat
        [HttpGet]
        public IEnumerable<string> GetMessages()
        {
            _chatHubContext.Clients.All.BroadcastMessage("test", "test");
            return new string[] { "value1", "value2" };
        }

        // GET: api/Chat/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Chat
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Chat/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
