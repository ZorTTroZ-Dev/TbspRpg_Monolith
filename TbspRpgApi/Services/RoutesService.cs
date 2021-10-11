using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Adapters;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services
{
    public interface IRoutesService
    {
        Task<List<RouteViewModel>> GetRoutes(RouteFilterRequest routeFilterRequest);
    }
    
    public class RoutesService : IRoutesService
    {
        private TbspRpgDataLayer.Services.IRoutesService _routesService;
        private readonly ILogger<RoutesService> _logger;

        public RoutesService(TbspRpgDataLayer.Services.IRoutesService routesService,
            ILogger<RoutesService> logger)
        {
            _routesService = routesService;
            _logger = logger;
        }
        
        public async Task<List<RouteViewModel>> GetRoutes(RouteFilterRequest routefilterRequest)
        {
            // TODO: Add security to return only routes associated with owned adventures
            // Included the Location and the destination location in the results
            // the filter out routes that aren't in their owned adventures
            var routes = await _routesService.GetRoutes(
                RouteFilterAdapter.ToDataLayerFilter(routefilterRequest));
            return routes.Select(route => new RouteViewModel(route)).ToList();
        }
    }
}