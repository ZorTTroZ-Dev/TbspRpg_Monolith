using System;

namespace TbspRpgApi.Entities
{
    public class Route
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SuccessSourceKey { get; set; }
        public Guid FailureSourceKey { get; set; }
        public Guid LocationId { get; set; }
        public Guid DestinationLocationId { get; set; }
        
        public Location Location { get; set; }
        public Location DestinationLocation { get; set; }
    }
}