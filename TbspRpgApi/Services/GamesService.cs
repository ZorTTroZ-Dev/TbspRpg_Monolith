using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
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
        Task<GameRouteListContentViewModel> StartGame(Guid userId, Guid adventureId, DateTime timeStamp);
        Task<GameViewModel> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId);
        Task<List<GameUserViewModel>> GetGames(GameFilterRequest gameFilterRequest);
        Task DeleteGame(Guid gameId);
        Task<JsonObject> GetGameState(Guid gameId);
        Task UpdateGameState(GameStateUpdateRequest updateRequest);
    }
    
    public class GamesService : IGamesService
    {
        private readonly TbspRpgDataLayer.Services.IGamesService _gamesService;
        private readonly IContentsService _contentsService;
        private readonly IMapsService _mapsService;
        private readonly ITbspRpgProcessor _tbspRpgProcessor;
        private readonly ILogger<GamesService> _logger;

        public GamesService(
            ITbspRpgProcessor tbspRpgProcessor,
            TbspRpgDataLayer.Services.IGamesService gamesService,
            IContentsService contentsService,
            IMapsService mapsService,
            ILogger<GamesService> logger)
        {
            _tbspRpgProcessor = tbspRpgProcessor;
            _gamesService = gamesService;
            _contentsService = contentsService;
            _mapsService = mapsService;
            _logger = logger;
        }

        public async Task<GameRouteListContentViewModel> StartGame(Guid userId, Guid adventureId, DateTime timeStamp)
        {
            var game = await _tbspRpgProcessor.StartGame(new GameStartModel()
            {
                UserId = userId,
                AdventureId = adventureId,
                TimeStamp = timeStamp
            });
            var routesViewModelTask = _mapsService.GetCurrentRoutesForGame(game.Id);
            var position = new DateTimeOffset(timeStamp).ToUnixTimeMilliseconds() - 1;
            var contentViewModelTask = _contentsService.GetContentForGameAfterPosition(game.Id, (ulong)position);
            return new GameRouteListContentViewModel()
            {
                Game = new GameViewModel(game),
                Routes = await routesViewModelTask,
                Contents = await contentViewModelTask
            };
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

        public async Task<JsonObject> GetGameState(Guid gameId)
        {
            var game = await _gamesService.GetGameById(gameId);
            game.LoadGameStateJson();
            return game.GameStateJson;
        }

        public async Task UpdateGameState(GameStateUpdateRequest updateRequest)
        {
            var game = await _gamesService.GetGameById(updateRequest.GameId);
            game.GameState = updateRequest.GameState;
            await _gamesService.SaveChanges();
        }
    }
}