using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors;

public interface IAdventureObjectProcessor
{
    Task RemoveAdventureObject(AdventureObjectRemoveModel adventureObjectRemoveModel);
}

public class AdventureObjectProcessor: IAdventureObjectProcessor
{
    private readonly IAdventureObjectService _adventureObjectService;
    private readonly ILogger _logger;

    public AdventureObjectProcessor(
        IAdventureObjectService adventureObjectService,
        ILogger logger)
    {
        _adventureObjectService = adventureObjectService;
        _logger = logger;
    }

    public async Task RemoveAdventureObject(AdventureObjectRemoveModel adventureObjectRemoveModel)
    {
        var dbAdventureObject =
            await _adventureObjectService.GetAdventureObjectById(adventureObjectRemoveModel.AdventureObjectId);
        if (dbAdventureObject == null)
        {
            throw new ArgumentException("invalid adventure object id");
        }
        
        // eventually we'll have to deal with adventure object game state
        
        _adventureObjectService.RemoveAdventureObject(dbAdventureObject);
        await _adventureObjectService.SaveChanges();
    }
}