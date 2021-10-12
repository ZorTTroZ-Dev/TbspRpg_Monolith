using TbspRpgApi.RequestModels;
using TbspRpgDataLayer.ArgumentModels;

namespace TbspRpgApi.Adapters
{
    public static class RouteFilterAdapter
    {
        public static RouteFilter ToDataLayerFilter(RouteFilterRequest routeFilterRequest)
        {
            return new RouteFilter()
            {
                LocationId = routeFilterRequest.LocationId
            };
        }
    }
}