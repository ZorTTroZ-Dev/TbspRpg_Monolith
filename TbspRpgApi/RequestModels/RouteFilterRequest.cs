using System;

namespace TbspRpgApi.RequestModels
{
    public class RouteFilterRequest
    {
        public Guid? Id { get; set; }
        public Guid? LocationId { get; set; }
    }
}