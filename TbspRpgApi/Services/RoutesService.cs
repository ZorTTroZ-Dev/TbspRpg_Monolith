using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.Services
{
    public interface IRoutesService
    {
        Task<List<RouteViewModel>> GetRoutes(RouteFilterRequest routeFilterRequest);
        Task<RouteViewModel> GetRouteById(Guid routeId);
        Task UpdateRoutesWithSource(ICollection<RouteUpdateRequest> updateRouteRequests);
        Task UpdateRouteWithSource(RouteUpdateRequest updateRouteRequest);
        Task DeleteRoute(Guid routeId);
    }
    
    public class RoutesService : IRoutesService
    {
        private readonly ITbspRpgProcessor _tbspRpgProcessor;
        private readonly TbspRpgDataLayer.Services.IRoutesService _routesService;
        private readonly ILogger<RoutesService> _logger;

        public RoutesService(
            ITbspRpgProcessor tbspRpgProcessor,
            TbspRpgDataLayer.Services.IRoutesService routesService,
            ILogger<RoutesService> logger)
        {
            _tbspRpgProcessor = tbspRpgProcessor;
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

        public async Task UpdateRoutesWithSource(ICollection<RouteUpdateRequest> updateRouteRequests)
        {
            // remove any routes
            var routeIds = updateRouteRequests.Select(routeRequest => routeRequest.route.Id).ToList();
            await _tbspRpgProcessor.RemoveRoutes(routeIds,
                updateRouteRequests.First().route.LocationId);
            
            foreach (var updateRouteRequest in updateRouteRequests)
            {
                await _tbspRpgProcessor.UpdateRoute(updateRouteRequest.ToRouteUpdateModel());
            }
        }

        public async Task UpdateRouteWithSource(RouteUpdateRequest updateRouteRequest)
        {
            await _tbspRpgProcessor.UpdateRoute(updateRouteRequest.ToRouteUpdateModel());
        }

        public async Task DeleteRoute(Guid routeId)
        {
            await _tbspRpgProcessor.RemoveRoute(new RouteRemoveModel()
            {
                RouteId = routeId
            });
        }
    }
}