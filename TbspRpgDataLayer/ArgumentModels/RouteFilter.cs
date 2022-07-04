using System;

namespace TbspRpgDataLayer.ArgumentModels
{
    public class RouteFilter
    {
        public Guid? LocationId { get; set; }
        public Guid? DestinationLocationId { get; set; }
        public Guid? AdventureId { get; set; }
    }
}