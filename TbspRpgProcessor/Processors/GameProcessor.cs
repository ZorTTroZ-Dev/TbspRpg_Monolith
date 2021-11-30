using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IGameProcessor
    {
        Task<Game> StartGame(Guid userId, Guid adventureId, DateTime timeStamp);
        Task DeleteGame(GameDeleteModel gameDeleteModel);
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

        public async Task DeleteGame(GameDeleteModel gameDeleteModel)
        {
            var game = await _gamesService.GetGameById(gameDeleteModel.GameId);
            if(game == null)
                throw new ArgumentException("invalid game id");
            
            // delete any associated content
            var contents = await _contentsService.GetAllContentForGame(game.Id);
            _contentsService.RemoveContents(contents);
            // delete the game row
            _gamesService.RemoveGame(game);
            await _gamesService.SaveChanges();
        }
    }
}