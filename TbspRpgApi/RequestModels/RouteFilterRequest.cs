using System;
using TbspRpgDataLayer.ArgumentModels;

namespace TbspRpgApi.RequestModels
{
    public class RouteFilterRequest
    {
        public Guid? LocationId { get; set; }
        public Guid? DestinationLocationId { get; set; }
        public Guid? AdventureId { get; set; }

        public RouteFilter ToRouteFilter()
        {
            return new RouteFilter()
            {
                LocationId = LocationId,
                DestinationLocationId = DestinationLocationId,
                AdventureId = AdventureId
            };
        }
    }
}