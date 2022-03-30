using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface IRoutesRepository : IBaseRepository
    {
        Task<List<Route>> GetRoutesForLocation(Guid locationId);
        Task<Route> GetRouteById(Guid routeId);
        Task<List<Route>> GetRoutes(RouteFilter routeFilter);
        void RemoveRoute(Route route);
        void RemoveRoutes(ICollection<Route> routes);
        Task AddRoute(Route route);
    }
    
    public class RoutesRepository : IRoutesRepository
    {
        private readonly DatabaseContext _databaseContext;

        public RoutesRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<Route>> GetRoutes(RouteFilter routeFilter)
        {
            var query = _databaseContext.Routes.AsQueryable();
            if (routeFilter != null)
            {
                if (routeFilter.DestinationLocationId != null)
                {
                    query = query.Where(route => route.DestinationLocationId == routeFilter.DestinationLocationId);
                }
                if (routeFilter.LocationId != null)
                {
                    query = query.Where(route => route.LocationId == routeFilter.LocationId);
                }    
            }
            return query.ToListAsync();
        }

        public void RemoveRoute(Route route)
        {
            _databaseContext.Remove(route);
        }

        public void RemoveRoutes(ICollection<Route> routes)
        {
            _databaseContext.RemoveRange(routes);
        }

        public async Task AddRoute(Route route)
        {
            await _databaseContext.AddAsync(route);
        }

        public Task<List<Route>> GetRoutesForLocation(Guid locationId)
        {
            return GetRoutes(new RouteFilter()
            {
                LocationId = locationId
            });
        }

        public Task<Route> GetRouteById(Guid routeId)
        {
            return _databaseContext.Routes.AsQueryable().
                Include(route => route.DestinationLocation).
                FirstOrDefaultAsync(route => route.Id == routeId);
        }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}