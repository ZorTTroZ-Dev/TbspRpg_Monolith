using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IAdventureProcessor
    {
        Task UpdateAdventure(AdventureUpdateModel adventureUpdateModel);
    }
    
    public class AdventureProcessor: IAdventureProcessor
    {
        private readonly ISourceProcessor _sourceProcessor;
        private readonly IAdventuresService _adventuresService;
        private readonly ILogger<AdventureProcessor> _logger;

        public AdventureProcessor(ISourceProcessor sourceProcessor,
            IAdventuresService adventuresService,
            ILogger<AdventureProcessor> logger)
        {
            _sourceProcessor = sourceProcessor;
            _adventuresService = adventuresService;
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
                    CreatedByUserId = adventureUpdateModel.UserId
                };
                await _adventuresService.AddAdventure(adventure);
            }
            else
            {
                // look up the adventure and update the name
                var dbAdventure = await _adventuresService.GetAdventureById(adventureUpdateModel.Adventure.Id);
                if (dbAdventure == null)
                    throw new ArgumentException("invalid adventure id");
                dbAdventure.Name = adventureUpdateModel.Adventure.Name;
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
    }
}