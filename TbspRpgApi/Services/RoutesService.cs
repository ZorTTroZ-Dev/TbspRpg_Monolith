using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Processors;

namespace TbspRpgApi.Services
{
    public interface IRoutesService
    {
        Task<List<RouteViewModel>> GetRoutes(RouteFilterRequest routeFilterRequest);
        Task<RouteViewModel> GetRouteById(Guid routeId);
        Task UpdateRoutesWithSource(ICollection<UpdateRouteRequest> updateRouteRequests);
    }
    
    public class RoutesService : IRoutesService
    {
        private readonly IRouteProcessor _routeProcessor;
        private readonly TbspRpgDataLayer.Services.IRoutesService _routesService;
        private readonly ILogger<RoutesService> _logger;

        public RoutesService(
            IRouteProcessor routeProcessor,
            TbspRpgDataLayer.Services.IRoutesService routesService,
            ILogger<RoutesService> logger)
        {
            _routeProcessor = routeProcessor;
            _routesService = routesService;
            _logger = logger;
        }
        
        public async Task<List<RouteViewModel>> GetRoutes(RouteFilterRequest routefilterRequest)
        {
            var routes = await _routesService.GetRoutes(routefilterRequest.ToRouteFilter());
            return routes.Select(route => new RouteViewModel(route)).ToList();
        }

        public async Task<RouteViewModel> GetRouteById(Guid routeId)
        {
            var route = await _routesService.GetRouteById(routeId);
            return route != null ? new RouteViewModel(route) : null;
        }

        public async Task UpdateRoutesWithSource(ICollection<UpdateRouteRequest> updateRouteRequests)
        {
            // remove any routes
            var routeIds = updateRouteRequests.Select(routeRequest => routeRequest.route.Id).ToList();
            await _routeProcessor.RemoveRoutes(routeIds,
                updateRouteRequests.First().route.LocationId);
            
            foreach (var updateRouteRequest in updateRouteRequests)
            {
                await _routeProcessor.UpdateRoute(updateRouteRequest.ToRouteUpdateModel());
            }
        }
    }
}