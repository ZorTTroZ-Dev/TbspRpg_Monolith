using System;

namespace TbspRpgApi.RequestModels
{
    public class RouteFilterRequest
    {
        public Guid? LocationId { get; set; }
        public Guid? DestinationLocationId { get; set; }
    }
}