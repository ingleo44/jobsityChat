using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Classes
{
    public class JwtToken
    {
        public string Token { get; set; }
        public DateTime ExpDate { get; set; }
        public string RefreshToken { get; set; }
    }
}
