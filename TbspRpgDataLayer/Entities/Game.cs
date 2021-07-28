using System;
using System.Collections.Generic;

namespace TbspRpgApi.Entities
{
    public class Game
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid AdventureId { get; set; }
        
        public Adventure Adventure { get; set; }
        public User User { get; set; }
        public ICollection<Content> Contents { get; set; }
    }
}