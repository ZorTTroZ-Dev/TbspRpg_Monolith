using System;
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
        Task<RouteViewModel> GetRouteById(Guid routeId);
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
            var routes = await _routesService.GetRoutes(
                RouteFilterAdapter.ToDataLayerFilter(routefilterRequest));
            return routes.Select(route => new RouteViewModel(route)).ToList();
        }

        public async Task<RouteViewModel> GetRouteById(Guid routeId)
        {
            var route = await _routesService.GetRouteById(routeId);
            return route != null ? new RouteViewModel(route) : null;
        }
    }
}