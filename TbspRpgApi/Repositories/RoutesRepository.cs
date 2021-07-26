using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;

namespace TbspRpgApi.Repositories
{
    public interface IRoutesRepository
    {
        Task<List<Route>> GetRoutesForLocation(Guid locationId);
    }
    
    public class RoutesRepository : IRoutesRepository
    {
        private readonly DatabaseContext _databaseContext;

        public RoutesRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<Route>> GetRoutesForLocation(Guid locationId)
        {
            return _databaseContext.Routes.AsQueryable().Where(rt => rt.LocationId == locationId).ToListAsync();
        }
    }
}