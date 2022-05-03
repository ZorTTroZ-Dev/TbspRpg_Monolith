using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IGameProcessor
    {
        Task<Game> StartGame(Guid userId, Guid adventureId, DateTime timeStamp);
        Task RemoveGame(GameRemoveModel gameRemoveModel);
        Task RemoveGame(Game game, bool save = true);
        Task RemoveGames(ICollection<Game> games, bool save = true);
    }
    
    public class GameProcessor : IGameProcessor
    {
        private readonly IAdventuresService _adventuresService;
        private readonly IUsersService _usersService;
        private readonly IGamesService _gamesService;
        private readonly ILocationsService _locationsService;
        private readonly IContentsService _contentsService;
        private readonly IScriptProcessor _scriptProcessor;
        private readonly ILogger<GameProcessor> _logger;

        public GameProcessor(
            IScriptProcessor scriptProcessor,
            IAdventuresService adventuresService,
            IUsersService usersService,
            IGamesService gamesService,
            ILocationsService locationsService,
            IContentsService contentsService,
            ILogger<GameProcessor> logger)
        {
            _scriptProcessor = scriptProcessor;
            _adventuresService = adventuresService;
            _usersService = usersService;
            _gamesService = gamesService;
            _locationsService = locationsService;
            _contentsService = contentsService;
            _logger = logger;
        }
        
        public async Task<Game> StartGame(Guid userId, Guid adventureId, DateTime timeStamp)
        {
            var user = await _usersService.GetById(userId);
            if (user == null)
                throw new ArgumentException("invalid user id");

            var adventure = await _adventuresService.GetAdventureById(adventureId);
            if (adventure == null)
                throw new ArgumentException("invalid adventure id");
            
            // check if the user already has a game of this adventure
            var game = await _gamesService.GetGameByAdventureIdAndUserId(adventure.Id, user.Id);
            if (game != null)
                return game;
            
            // get the initial location for the game
            var location = await _locationsService.GetInitialLocationForAdventure(adventure.Id);
            if (location == null)
                throw new Exception("no initial location for adventure");
            
            // add game to the context
            var secondsSinceEpoch = new DateTimeOffset(timeStamp).ToUnixTimeMilliseconds();
            game = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = adventure.Id,
                UserId = user.Id,
                LocationId = location.Id,
                LocationUpdateTimeStamp = secondsSinceEpoch
            };
            await _gamesService.AddGame(game);
            
            // run the initialization script for the adventure if there is one
            if (adventure.InitializationScriptId != null)
            {
                await _scriptProcessor.ExecuteScript(adventure.InitializationScriptId.GetValueOrDefault());
            }

            // create content entry for adventure's source key
            await _contentsService.AddContent(new Content()
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                Position = (ulong)secondsSinceEpoch,
                SourceKey = adventure.InitialSourceKey
            });
            
            // create content entry for the initial location source key
            await _contentsService.AddContent(new Content()
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                Position = (ulong)secondsSinceEpoch + 1,
                SourceKey = location.SourceKey
            });

            // save context changes
            await _gamesService.SaveChanges();
            return game;
        }

        public async Task RemoveGame(GameRemoveModel gameRemoveModel)
        {
            var game = await _gamesService.GetGameById(gameRemoveModel.GameId);
            if(game == null)
                throw new ArgumentException("invalid game id");

            await RemoveGame(game);
        }

        public async Task RemoveGame(Game game, bool save = true)
        {
            // delete any associated content
            await _contentsService.RemoveAllContentsForGame(game.Id);
            // delete the game row
            _gamesService.RemoveGame(game);
            
            if(save)
                await _gamesService.SaveChanges();
        }

        public async Task RemoveGames(ICollection<Game> games, bool save = true)
        {
            foreach (var game in games)
            {
                await RemoveGame(game, false);
            }

            if (save)
                await _gamesService.SaveChanges();
        }
    }
}