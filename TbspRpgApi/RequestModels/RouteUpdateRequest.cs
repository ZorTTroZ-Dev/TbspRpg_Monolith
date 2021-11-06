using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels
{
    public class RouteUpdateRequest
    {
        public RouteViewModel route { get; set; }
        public SourceViewModel source { get; set; }
        public SourceViewModel successSource { get; set; }
        public string newDestinationLocationName { get; set; }

        public RouteUpdateModel ToRouteUpdateModel()
        {
            return new RouteUpdateModel()
            {
                route = route.ToEntity(),
                source = source.ToEntity(),
                successSource = successSource.ToEntity(),
                newDestinationLocationName = newDestinationLocationName,
                language = source.Language
            };
        }
    }
}
