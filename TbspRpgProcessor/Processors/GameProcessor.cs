using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Services;

namespace TbspRpgProcessor.Processors
{
    public interface IGameProcessor
    {
        Task<Game> StartGame(Guid userId, Guid adventureId, DateTime timeStamp);
    }
    
    public class GameProcessor : IGameProcessor
    {
        private readonly IAdventuresService _adventuresService;
        private readonly IUsersService _usersService;
        private readonly IGamesService _gamesService;
        private readonly ILocationsService _locationsService;
        private readonly IContentsService _contentsService;
        private readonly ILogger<GameProcessor> _logger;

        public GameProcessor(
            IAdventuresService adventuresService,
            IUsersService usersService,
            IGamesService gamesService,
            ILocationsService locationsService,
            IContentsService contentsService,
            ILogger<GameProcessor> logger)
        {
            _adventuresService = adventuresService;
            _usersService = usersService;
            _gamesService = gamesService;
            _locationsService = locationsService;
            _contentsService = contentsService;
            _logger = logger;
        }
        
        public async Task<Game> StartGame(Guid userId, Guid adventureId, DateTime timeStamp)
        {
            // make sure the ids are valid
            var userTask = _usersService.GetById(userId);
            var adventureTask = _adventuresService.GetAdventureById(adventureId);

            var user = await userTask;
            if (user == null)
                throw new ArgumentException("invalid user id");

            var adventure = await adventureTask;
            if (adventure == null)
                throw new ArgumentException("invalid adventure id");
            
            // check if the user already has a game of this adventure
            var game = await _gamesService.GetGameByAdventureIdAndUserId(adventure.Id, user.Id);
            if (game != null)
                throw new Exception("user already has an instance of given adventure");
            
            // get the initial location for the game
            var location = await _locationsService.GetInitialLocationForAdventure(adventure.Id);
            if (location == null)
                throw new Exception("no initial location for adventure");
            
            // add game to the context
            game = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = adventure.Id,
                UserId = user.Id,
                LocationId = location.Id
            };
            await _gamesService.AddGame(game);
            
            // create content entry for adventure's source key
            var secondsSinceEpoch = new DateTimeOffset(timeStamp).ToUnixTimeMilliseconds();
            await _contentsService.AddContent(new Content()
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                Position = (ulong)secondsSinceEpoch,
                SourceId = adventure.SourceKey
            });
            
            // create content entry for the initial location source key
            await _contentsService.AddContent(new Content()
            {
                Id = Guid.NewGuid(),
                GameId = game.Id,
                Position = (ulong)secondsSinceEpoch + 1,
                SourceId = location.SourceKey
            });

            // save context changes
            await _gamesService.SaveChanges();
            return game;
        }
    }
}