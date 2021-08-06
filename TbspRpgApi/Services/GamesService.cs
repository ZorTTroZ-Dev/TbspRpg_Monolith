using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgProcessor.Processors;

namespace TbspRpgApi.Services
{
    public interface IGamesService
    {
        Task StartGame(Guid userId, Guid adventureId, DateTime timeStamp);
    }
    
    public class GamesService : IGamesService
    {
        private readonly TbspRpgDataLayer.Services.IGamesService _gamesService;
        private readonly IGameProcessor _gameProcessor;
        private readonly ILogger<GamesService> _logger;

        public GamesService(TbspRpgDataLayer.Services.IGamesService gamesService,
            IGameProcessor gameProcessor,
            ILogger<GamesService> logger)
        {
            _gamesService = gamesService;
            _gameProcessor = gameProcessor;
            _logger = logger;
        }

        public async Task StartGame(Guid userId, Guid adventureId, DateTime timeStamp)
        {
            //this may eventually become sending a message to rabbit mq or another
            //messaging service which will then pass messages to worker processes
            //for now we're calling directly
            await _gameProcessor.StartGame(userId, adventureId, timeStamp);
        }
    }
}