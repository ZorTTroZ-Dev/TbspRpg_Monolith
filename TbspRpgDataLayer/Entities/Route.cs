using System;

namespace TbspRpgDataLayer.Entities
{
    public class Route
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SourceKey { get; set; }
        public Guid RouteTakenSourceKey { get; set; }
        public Guid LocationId { get; set; }
        public Guid DestinationLocationId { get; set; }
        public Guid? RouteTakenScriptId { get; set; }
        
        public Location Location { get; set; }
        public Location DestinationLocation { get; set; }
        public Script RouteTakenScript { get; set; }
    }
}