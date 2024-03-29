using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.Services;

public interface IObjectsService
{
    Task<List<ObjectViewModel>> GetObjectsForAdventure(Guid adventureId);
    Task<List<ObjectViewModel>> GetObjectsByLocation(Guid locationId);
    Task UpdateObject(ObjectUpdateRequest objectUpdateRequest);
    Task DeleteObject(Guid objectId);
}

public class ObjectsService: IObjectsService
{
    private readonly ITbspRpgProcessor _tbspRpgProcessor;
    private readonly IAdventureObjectService _adventureObjectService;
    private readonly ILogger<ScriptsService> _logger;
    
    public ObjectsService(
        ITbspRpgProcessor tbspRpgProcessor,
        IAdventureObjectService adventureObjectService,
        ILogger<ScriptsService> logger)
    {
        _tbspRpgProcessor = tbspRpgProcessor;
        _adventureObjectService = adventureObjectService;
        _logger = logger;
    }
    
    public async Task<List<ObjectViewModel>> GetObjectsForAdventure(Guid adventureId)
    {
        var objects = await _adventureObjectService
            .GetAdventureObjectsForAdventure(adventureId);
        return objects.Select(aObject => new ObjectViewModel(aObject)).ToList();
    }

    public async Task<List<ObjectViewModel>> GetObjectsByLocation(Guid locationId)
    {
        var objects = await _adventureObjectService.GetAdventureObjectsByLocation(locationId);
        return objects.Select(aObject => new ObjectViewModel(aObject)).ToList();
    }

    public async Task UpdateObject(ObjectUpdateRequest objectUpdateRequest)
    {
        await _tbspRpgProcessor.UpdateAdventureObject(objectUpdateRequest.ToAdventureObjectUpdateModel());
    }

    public async Task DeleteObject(Guid objectId)
    {
        await _tbspRpgProcessor.RemoveAdventureObject(new AdventureObjectRemoveModel()
        {
            AdventureObjectId = objectId
        });
    }
}