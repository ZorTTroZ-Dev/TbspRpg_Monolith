using System;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels
{
    public class RouteUpdateRequest
    {
        public RouteUpdateViewModel route { get; set; }
        public SourceViewModel source { get; set; }
        public SourceViewModel successSource { get; set; }

        public RouteUpdateModel ToRouteUpdateModel()
        {
            return new RouteUpdateModel()
            {
                route = route.ToEntity(),
                source = source.ToEntity(),
                successSource = successSource.ToEntity(),
                newDestinationLocationName = route.newDestinationLocationName,
                language = source.Language
            };
        }
    }
}
