using System;

namespace TbspRpgDataLayer.ArgumentModels
{
    public class RouteFilterRequest
    {
        public Guid? Id { get; set; }
        public Guid? LocationId { get; set; }
    }
}