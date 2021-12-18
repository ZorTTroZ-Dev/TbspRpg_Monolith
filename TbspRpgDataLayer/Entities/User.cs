using System;
using System.Collections.Generic;

namespace TbspRpgDataLayer.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RegistrationKey { get; set; }
        public bool RegistrationComplete { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastLogin { get; set; }
        
        public ICollection<Game> Games { get; set; }
        public ICollection<Adventure> Adventures { get; set; } 
        public ICollection<Group> Groups { get; set; }
    }
}