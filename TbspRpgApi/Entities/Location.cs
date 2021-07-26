using System;
using System.Collections.Generic;

namespace TbspRpgApi.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Initial { get; set; }
        public Guid SourceKey { get; set; }
        public Guid AdventureId { get; set; }
        
        public Adventure Adventure { get; set; }
        public ICollection<Route> Routes { get; set; }
    }
}