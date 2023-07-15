using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer.Repositories;

public interface IAdventureObjectRepository: IBaseRepository
{
    Task<AdventureObject> GetAdventureObjectById(Guid adventureObjectId);
    Task<List<AdventureObject>> GetAdventureObjectsForAdventure(Guid adventureId);
    Task AddAdventureObject(AdventureObject adventureObject);
    void RemoveAdventureObject(AdventureObject adventureObject);
    void RemoveAdventureObjects(ICollection<AdventureObject> adventureObjects);
}

public class AdventureObjectRepository: IAdventureObjectRepository
{
    private readonly DatabaseContext _databaseContext;
    
    public AdventureObjectRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task SaveChanges()
    {
        await _databaseContext.SaveChangesAsync();
    }

    public Task<AdventureObject> GetAdventureObjectById(Guid adventureObjectId)
    {
        return _databaseContext.AdventureObjects.AsQueryable()
            .FirstOrDefaultAsync(ao => ao.Id == adventureObjectId);
    }

    public Task<List<AdventureObject>> GetAdventureObjectsForAdventure(Guid adventureId)
    {
        return _databaseContext.AdventureObjects.AsQueryable()
            .Where(ao => ao.AdventureId == adventureId)
            .ToListAsync();
    }
    
    public async Task AddAdventureObject(AdventureObject adventureObject)
    {
        await _databaseContext.AdventureObjects.AddAsync(adventureObject);
    }
    
    public void RemoveAdventureObject(AdventureObject adventureObject)
    {
        _databaseContext.Remove(adventureObject);
    }

    public void RemoveAdventureObjects(ICollection<AdventureObject> adventureObjects)
    {
        _databaseContext.RemoveRange(adventureObjects);
    }
}