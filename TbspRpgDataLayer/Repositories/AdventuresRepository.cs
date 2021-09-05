using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;

namespace TbspRpgDataLayer.Repositories
{
    public interface IAdventuresRepository
    {
        Task<List<Adventure>> GetAllAdventures(AdventureFilter filters);
        Task<Adventure> GetAdventureByName(string name);
        Task<Adventure> GetAdventureById(Guid adventureId);
    }
    
    public class AdventuresRepository : IAdventuresRepository
    {
        private readonly DatabaseContext _databaseContext;

        public AdventuresRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<List<Adventure>> GetAllAdventures(AdventureFilter filters)
        {
            var query = _databaseContext.Adventures.AsQueryable();
            if (filters != null && filters.CreatedBy != Guid.Empty)
            {
                query = query.Where(a => a.CreatedByUserId == filters.CreatedBy);
            }
            return query.ToListAsync();
        }

        public Task<Adventure> GetAdventureByName(string name)
        {
            return _databaseContext.Adventures.AsQueryable().
                Where(a => a.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();
        }

        public Task<Adventure> GetAdventureById(Guid adventureId)
        {
            return _databaseContext.Adventures.AsQueryable().
                Where(a => a.Id == adventureId).
                FirstOrDefaultAsync();
        }
    }
}