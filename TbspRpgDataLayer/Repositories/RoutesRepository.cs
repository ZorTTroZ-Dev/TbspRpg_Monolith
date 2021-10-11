using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;

namespace TbspRpgDataLayer.Repositories
{
    public interface IRoutesRepository
    {
        Task<List<Route>> GetRoutesForLocation(Guid locationId);
        Task<Route> GetRouteById(Guid routeId);
        Task<List<Route>> GetRoutes(RouteFilter routeFilter);
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
                if (routeFilter.Id != null)
                {
                    query = query.Where(route => route.Id == routeFilter.Id);
                }

                if (routeFilter.LocationId != null)
                {
                    query = query.Where(route => route.LocationId == routeFilter.LocationId);
                }    
            }
            return query.ToListAsync();
        }

        public Task<List<Route>> GetRoutesForLocation(Guid locationId)
        {
            return this.GetRoutes(new RouteFilter()
            {
                Id = null,
                LocationId = locationId
            });
        }

        public Task<Route> GetRouteById(Guid routeId)
        {
            return _databaseContext.Routes.AsQueryable().
                Include(route => route.DestinationLocation).
                FirstOrDefaultAsync(route => route.Id == routeId);
        }
    }
}