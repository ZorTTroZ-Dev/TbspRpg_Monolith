using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IContentProcessor
    {
        Task<string> GetContentTextForKey(Guid gameId, Guid sourceKey, bool processed = false);
    }
    
    public class ContentProcessor : IContentProcessor
    {
        private readonly IGamesService _gamesService;
        private readonly ISourceProcessor _sourceProcessor;
        private readonly ILogger _logger;

        public ContentProcessor(
            IGamesService gamesService,
            ISourceProcessor sourceProcessor,
            ILogger logger)
        {
            _gamesService = gamesService;
            _sourceProcessor = sourceProcessor;
            _logger = logger;
        }

        public async Task<string> GetContentTextForKey(Guid gameId, Guid sourceKey, bool processed = false)
        {
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