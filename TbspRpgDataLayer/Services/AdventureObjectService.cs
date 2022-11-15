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
        await _adventureObjectRepository.SaveChanges();
    }

    public Task<AdventureObject> GetAdventureObjectById(Guid adventureObjectId)
    {
        return _adventureObjectRepository.GetAdventureObjectById(adventureObjectId);
    }

    public Task<List<AdventureObject>> GetAdventureObjectsForAdventure(Guid adventureId)
    {
        return _adventureObjectRepository.GetAdventureObjectsForAdventure(adventureId);
    }

    public async Task AddAdventureObject(AdventureObject adventureObject)
    {
        await _adventureObjectRepository.AddAdventureObject(adventureObject);
    }

    public void RemoveAdventureObject(AdventureObject adventureObject)
    {
        _adventureObjectRepository.RemoveAdventureObject(adventureObject);
    }

    public void RemoveAdventureObjects(ICollection<AdventureObject> adventureObjects)
    {
        _adventureObjectRepository.RemoveAdventureObjects(adventureObjects);
    }
}