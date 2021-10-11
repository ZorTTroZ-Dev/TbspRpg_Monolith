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
        Task<List<Route>> GetRoutes(RouteFilterRequest routeFilterRequest);
    }
    
    public class RoutesRepository : IRoutesRepository
    {
        private readonly DatabaseContext _databaseContext;

        public RoutesRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<Route>> GetRoutes(RouteFilterRequest routeFilterRequest)
        {
            var query = _databaseContext.Routes.AsQueryable();
            if (routeFilterRequest != null)
            {
                if (routeFilterRequest.Id != null)
                {
                    query = query.Where(route => route.Id == routeFilterRequest.Id);
                }

                if (routeFilterRequest.LocationId != null)
                {
                    query = query.Where(route => route.LocationId == routeFilterRequest.LocationId);
                }    
            }
            return query.ToListAsync();
        }

        public Task<List<Route>> GetRoutesForLocation(Guid locationId)
        {
            return this.GetRoutes(new RouteFilterRequest()
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