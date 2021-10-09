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
            throw new NotImplementedException();
        }

        public Task<List<Route>> GetRoutesForLocation(Guid locationId)
        {
            return _databaseContext.Routes.AsQueryable().Where(rt => rt.LocationId == locationId).ToListAsync();
        }

        public Task<Route> GetRouteById(Guid routeId)
        {
            return _databaseContext.Routes.AsQueryable().
                Include(route => route.DestinationLocation).
                FirstOrDefaultAsync(route => route.Id == routeId);
        }
    }
}