using System;

namespace Entities
{
    public class User
    {
        public int idUser { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public Guid Id { get; set; }
    }
}
