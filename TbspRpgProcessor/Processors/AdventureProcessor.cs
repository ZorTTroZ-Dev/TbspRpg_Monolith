using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IAdventureProcessor
    {
        Task UpdateAdventure(AdventureUpdateModel adventureUpdateModel);
        Task RemoveAdventure(AdventureRemoveModel adventureRemoveModel);
    }
    
    public class AdventureProcessor: IAdventureProcessor
    {
        private readonly ISourceProcessor _sourceProcessor;
        private readonly IGameProcessor _gameProcessor;
        private readonly ILocationProcessor _locationProcessor;
        private readonly IAdventuresService _adventuresService;
        private readonly ISourcesService _sourcesService;
        private readonly ILogger<AdventureProcessor> _logger;

        public AdventureProcessor(ISourceProcessor sourceProcessor,
            IGameProcessor gameProcessor,
            ILocationProcessor locationProcessor,
            IAdventuresService adventuresService,
            ISourcesService sourcesService,
            ILogger<AdventureProcessor> logger)
        {
            _sourceProcessor = sourceProcessor;
            _gameProcessor = gameProcessor;
            _locationProcessor = locationProcessor;
            _adventuresService = adventuresService;
            _sourcesService = sourcesService;
            _logger = logger;
        }
        
        public async Task UpdateAdventure(AdventureUpdateModel adventureUpdateModel)
        {
            // update/create a new adventure
            Adventure adventure = null;
            if (adventureUpdateModel.Adventure.Id == Guid.Empty)
            {
                // create a new adventure
                adventure = new Adventure()
                {
                    Id = Guid.NewGuid(),
                    Name = adventureUpdateModel.Adventure.Name,
                    InitialSourceKey = Guid.Empty,
                    CreatedByUserId = adventureUpdateModel.UserId,
                    PublishDate = adventureUpdateModel.Adventure.PublishDate
                };
                await _adventuresService.AddAdventure(adventure);
            }
            else
            {
                // look up the adventure and update the name and publish date
                var dbAdventure = await _adventuresService.GetAdventureById(adventureUpdateModel.Adventure.Id);
                if (dbAdventure == null)
                    throw new ArgumentException("invalid adventure id");
                dbAdventure.Name = adventureUpdateModel.Adventure.Name;
                dbAdventure.PublishDate = adventureUpdateModel.Adventure.PublishDate;
                adventure = dbAdventure;
            }
            
            // update/create initial source
            adventureUpdateModel.InitialSource.AdventureId = adventure.Id;
            if (string.IsNullOrEmpty(adventureUpdateModel.InitialSource.Name))
                adventureUpdateModel.InitialSource.Name = $"Initial{adventureUpdateModel.Adventure.Name}";
            var dbSource = await _sourceProcessor.CreateOrUpdateSource(
                adventureUpdateModel.InitialSource,
                adventureUpdateModel.Language);
            adventure.InitialSourceKey = dbSource.Key;
            
            // update/create description source
            adventureUpdateModel.DescriptionSource.AdventureId = adventure.Id;
            if (string.IsNullOrEmpty(adventureUpdateModel.InitialSource.Name))
                adventureUpdateModel.DescriptionSource.Name = $"Description{adventureUpdateModel.Adventure.Name}";
            var dbDescriptionSource = await _sourceProcessor.CreateOrUpdateSource(
                adventureUpdateModel.DescriptionSource,
                adventureUpdateModel.Language);
            adventure.DescriptionSourceKey = dbDescriptionSource.Key;

            await _adventuresService.SaveChanges();
        }

        public async Task RemoveAdventure(AdventureRemoveModel adventureRemoveModel)
        {
            // load the adventure from the database
            var adventure = await _adventuresService.GetAdventureByIdIncludeAssociatedObjects(adventureRemoveModel.AdventureId);
            if (adventure == null)
                throw new ArgumentException("invalid adventure id");
            
            await _gameProcessor.RemoveGames(adventure.Games, false);
            await _locationProcessor.RemoveLocations(adventure.Locations, false);
            await _sourcesService.RemoveAllSourceForAdventure(adventure.Id);
            _adventuresService.RemoveAdventure(adventure);
            await _adventuresService.SaveChanges();
        }
    }
}