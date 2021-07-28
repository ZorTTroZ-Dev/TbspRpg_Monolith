using System;
using System.Collections.Generic;

namespace TbspRpgApi.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        
        public ICollection<Game> Games { get; set; }
    }
}