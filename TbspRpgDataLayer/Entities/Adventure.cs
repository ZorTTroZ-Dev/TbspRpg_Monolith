using System;
using System.Collections.Generic;

namespace TbspRpgApi.Entities
{
    public class Adventure
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid InitialSourceKey { get; set; }
        public Guid CreatedByUserId { get; set; }
        
        public ICollection<Location> Locations { get; set; }
        public ICollection<Game> Games { get; set; }
        public User CreatedByUser { get; set; }
    }
}