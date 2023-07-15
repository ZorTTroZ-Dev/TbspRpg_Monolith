using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors;

public interface IAdventureObjectProcessor
{
    Task RemoveAdventureObject(AdventureObjectRemoveModel adventureObjectRemoveModel);
    Task UpdateAdventureObject(AdventureObjectUpdateModel adventureObjectUpdateModel);
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

    public async Task UpdateAdventureObject(AdventureObjectUpdateModel adventureObjectUpdateModel)
    {
        if (adventureObjectUpdateModel.adventureObject.Id == Guid.Empty)
        {
            var adventureObject = new AdventureObject()
            {
                Id = Guid.NewGuid(),
                Name = adventureObjectUpdateModel.adventureObject.Name,
                Description = adventureObjectUpdateModel.adventureObject.Description,
                Type = adventureObjectUpdateModel.adventureObject.Type,
                AdventureId = adventureObjectUpdateModel.adventureObject.AdventureId
            };
            await _adventureObjectService.AddAdventureObject(adventureObject);
        }
        else
        {
            var dbAdventureObject =
                await _adventureObjectService.GetAdventureObjectById(adventureObjectUpdateModel.adventureObject.Id);
            if (dbAdventureObject == null)
            {
                throw new ArgumentException("invalid adventure object id");
            }

            dbAdventureObject.Name = adventureObjectUpdateModel.adventureObject.Name;
            dbAdventureObject.Description = adventureObjectUpdateModel.adventureObject.Description;
            dbAdventureObject.Type = adventureObjectUpdateModel.adventureObject.Type;
        }

        await _adventureObjectService.SaveChanges();
    }
}