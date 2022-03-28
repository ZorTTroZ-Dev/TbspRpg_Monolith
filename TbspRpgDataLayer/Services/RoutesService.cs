using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface IRoutesService: IBaseService
    {
        Task<List<Route>> GetRoutesForLocation(Guid locationId);
        Task<Route> GetRouteById(Guid routeId);
        Task<List<Route>> GetRoutes(RouteFilter routeFilter);
        void RemoveRoute(Route route);
        void RemoveRoutes(ICollection<Route> routes);
        Task AddRoute(Route route);
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

        public void RemoveRoute(Route route)
        {
            _routesRepository.RemoveRoute(route);
        }

        public void RemoveRoutes(ICollection<Route> routes)
        {
            throw new NotImplementedException();
        }

        public async Task AddRoute(Route route)
        {
            await _routesRepository.AddRoute(route);
        }

        public async Task SaveChanges()
        {
            await _routesRepository.SaveChanges();
        }
    }
}