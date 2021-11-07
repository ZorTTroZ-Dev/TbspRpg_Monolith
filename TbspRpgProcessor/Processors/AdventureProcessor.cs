using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
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
            if (adventureUpdateModel.adventure.Id == Guid.Empty)
            {
                // create a new adventure
                adventure = new Adventure()
                {
                    Name = adventureUpdateModel.adventure.Name,
                    SourceKey = Guid.Empty,
                    CreatedByUserId = adventureUpdateModel.user.Id
                };
                await _adventuresService.AddAdventure(adventure);
            }
            else
            {
                // look up the adventure and update the name
                var dbAdventure = await _adventuresService.GetAdventureById(adventureUpdateModel.adventure.Id);
                if (dbAdventure == null)
                    throw new ArgumentException("invalid adventure id");
                dbAdventure.Name = adventureUpdateModel.adventure.Name;
                adventure = dbAdventure;
            }
            
            // update/create source
            adventureUpdateModel.source.AdventureId = adventure.Id;
            if (string.IsNullOrEmpty(adventureUpdateModel.source.Name))
                adventureUpdateModel.source.Name = adventureUpdateModel.adventure.Name;
            var dbSource = await _sourceProcessor.CreateOrUpdateSource(
                adventureUpdateModel.source,
                adventureUpdateModel.language);
            adventure.SourceKey = dbSource.Key;

            await _adventuresService.SaveChanges();
        }
    }
}