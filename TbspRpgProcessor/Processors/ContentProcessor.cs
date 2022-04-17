using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IContentProcessor
    {
        Task<string> GetSourceForKey(Guid gameId, Guid sourceKey, bool processed = false);
    }
    
    public class ContentProcessor : IContentProcessor
    {
        private readonly IGamesService _gamesService;
        private readonly ISourceProcessor _sourceProcessor;
        private readonly ILogger<ContentProcessor> _logger;

        public ContentProcessor(
            IGamesService gamesService,
            ISourceProcessor sourceProcessor,
            ILogger<ContentProcessor> logger)
        {
            _gamesService = gamesService;
            _sourceProcessor = sourceProcessor;
            _logger = logger;
        }

        public async Task<string> GetSourceForKey(Guid gameId, Guid sourceKey, bool processed = false)
        {
            // get the language the game is set to
            // eventually this will look up javascript
            // that will use the game state to return a source key for lookup
            // for now we'll just look up the source key
            var game = await _gamesService.GetGameById(gameId);
            if (game == null)
                throw new ArgumentException("invalid game id");

            var dbSource = await _sourceProcessor.GetSourceForKey(new SourceForKeyModel()
            {
                Key = sourceKey,
                AdventureId = game.AdventureId,
                Language = game.Language,
                Processed = processed
            });
            
            return dbSource?.Text;
        }
    }
}