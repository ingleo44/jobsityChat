using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public string User { get; set; }
        public string MessageText { get; set; }
        public DateTime SendDateTime { get; set; }
    }
}
