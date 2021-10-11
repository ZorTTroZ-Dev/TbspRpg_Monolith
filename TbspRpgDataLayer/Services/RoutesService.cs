using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface IRoutesService
    {
        Task<List<Route>> GetRoutesForLocation(Guid locationId);
        Task<Route> GetRouteById(Guid routeId);
        Task<List<Route>> GetRoutes(RouteFilter routeFilter);
    }
    
    public class RoutesService : IRoutesService
    {
        private readonly IRoutesRepository _routesRepository;
        private readonly ILogger<RoutesService> _logger;

        public RoutesService(IRoutesRepository routesRepository, ILogger<RoutesService> logger)
        {
            _routesRepository = routesRepository;
            _logger = logger;
        }

        public Task<List<Route>> GetRoutesForLocation(Guid locationId)
        {
            return _routesRepository.GetRoutesForLocation(locationId);
        }

        public Task<Route> GetRouteById(Guid routeId)
        {
            return _routesRepository.GetRouteById(routeId);
        }

        public Task<List<Route>> GetRoutes(RouteFilter routeFilter)
        {
            return _routesRepository.GetRoutes(routeFilter);
        }
    }
}