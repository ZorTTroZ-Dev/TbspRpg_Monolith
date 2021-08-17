using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Services;

namespace TbspRpgProcessor.Processors
{
    public interface IContentProcessor
    {
        Task<string> GetContentForKey(Guid gameId, Guid sourceKey);
    }
    
    public class ContentProcessor : IContentProcessor
    {
        private readonly IGamesService _gamesService;
        private readonly ISourcesService _sourcesService;
        private readonly ILogger<ContentProcessor> _logger;

        public ContentProcessor(
            IGamesService gamesService,
            ISourcesService sourcesService,
            ILogger<ContentProcessor> logger)
        {
            _gamesService = gamesService;
            _sourcesService = sourcesService;
            _logger = logger;
        }

        public async Task<string> GetContentForKey(Guid gameId, Guid sourceKey)
        {
            // get the language the game is set to
            // eventually this will look up javascript
            // that will use the game state to return a source key for lookup
            // for now we'll just look up the source key
            var game = await _gamesService.GetGameById(gameId);
            if (game == null)
                throw new ArgumentException("invalid game id");

            return await _sourcesService.GetSourceForKey(sourceKey, game.Language);
        }
    }
}