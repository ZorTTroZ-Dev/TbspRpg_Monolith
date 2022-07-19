using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.Services
{
    public interface IGamesService
    {
        Task StartGame(Guid userId, Guid adventureId, DateTime timeStamp);
        Task<GameViewModel> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId);
        Task<List<GameUserViewModel>> GetGames(GameFilterRequest gameFilterRequest);
        Task DeleteGame(Guid gameId);
    }
    
    public class GamesService : IGamesService
    {
        private readonly TbspRpgDataLayer.Services.IGamesService _gamesService;
        private readonly ITbspRpgProcessor _tbspRpgProcessor;
        private readonly ILogger<GamesService> _logger;

        public GamesService(
            ITbspRpgProcessor tbspRpgProcessor,
            TbspRpgDataLayer.Services.IGamesService gamesService,
            ILogger<GamesService> logger)
        {
            _tbspRpgProcessor = tbspRpgProcessor;
            _gamesService = gamesService;
            _logger = logger;
        }

        public async Task StartGame(Guid userId, Guid adventureId, DateTime timeStamp)
        {
            //this may eventually become sending a message to rabbit mq or another
            //messaging service which will then pass messages to worker processes
            //for now we're calling directly
            await _tbspRpgProcessor.StartGame(userId, adventureId, timeStamp);
        }

        public async Task<GameViewModel> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId)
        {
            var game = await _gamesService.GetGameByAdventureIdAndUserId(adventureId, userId);
            return game != null ? new GameViewModel(game) : null;
        }

        public async Task<List<GameUserViewModel>> GetGames(GameFilterRequest gameFilterRequest)
        {
            var games = await _gamesService.GetGamesIncludeUsers(gameFilterRequest.ToGameFilter());
            return games.Select(game => new GameUserViewModel()
            {
                Game = new GameViewModel(game),
                User = new UserViewModel(game.User)
            }).ToList();
        }

        public async Task DeleteGame(Guid gameId)
        {
            await _tbspRpgProcessor.RemoveGame(new GameRemoveModel()
            {
                GameId = gameId
            });
        }
    }
}