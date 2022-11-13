using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services;

public interface IAdventureObjectService : IBaseService
{
    Task<AdventureObject> GetAdventureObjectById(Guid adventureObjectId);
    Task<List<AdventureObject>> GetAdventureObjectsForAdventure(Guid adventureId);
    Task AddAdventureObject(AdventureObject adventureObject);
    void RemoveAdventureObject(AdventureObject adventureObject);
    void RemoveAdventureObjects(ICollection<AdventureObject> adventureObjects);
}

public class AdventureObjectService: IAdventureObjectService
{
    private readonly IAdventureObjectRepository _adventureObjectRepository;
    private readonly ILogger<AdventureObjectService> _logger;
    
    public AdventureObjectService(IAdventureObjectRepository adventureObjectRepository,
        ILogger<AdventureObjectService> logger)
    {
        _adventureObjectRepository = adventureObjectRepository;
        _logger = logger;
    }
    
    public async Task SaveChanges()
    {
        throw new System.NotImplementedException();
    }

    public async Task<AdventureObject> GetAdventureObjectById(Guid adventureObjectId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<AdventureObject>> GetAdventureObjectsForAdventure(Guid adventureId)
    {
        throw new NotImplementedException();
    }

    public async Task AddAdventureObject(AdventureObject adventureObject)
    {
        throw new NotImplementedException();
    }

    public void RemoveAdventureObject(AdventureObject adventureObject)
    {
        throw new NotImplementedException();
    }

    public void RemoveAdventureObjects(ICollection<AdventureObject> adventureObjects)
    {
        throw new NotImplementedException();
    }
}