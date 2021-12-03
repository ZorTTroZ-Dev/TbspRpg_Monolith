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
    public interface IAdventuresRepository: IBaseRepository
    {
        Task<List<Adventure>> GetAllAdventures(AdventureFilter filters);
        Task<Adventure> GetAdventureByName(string name);
        Task<Adventure> GetAdventureById(Guid adventureId);
        Task AddAdventure(Adventure adventure);
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

        public async Task AddAdventure(Adventure adventure)
        {
            await _databaseContext.AddAsync(adventure);
        }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}