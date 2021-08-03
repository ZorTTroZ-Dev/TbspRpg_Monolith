using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Services;

namespace TbspRpgProcessor.Processors
{
    public interface IGameProcessor
    {
        Task<Game> StartGame(Guid userId, Guid adventureId);
    }
    
    public class GameProcessor : IGameProcessor
    {
        private readonly IAdventuresService _adventuresService;
        private readonly IUsersService _usersService;
        private readonly IGamesService _gamesService;
        private readonly ILogger<GameProcessor> _logger;

        public GameProcessor(
            IAdventuresService adventuresService,
            IUsersService usersService,
            IGamesService gamesService,
            ILogger<GameProcessor> logger)
        {
            _adventuresService = adventuresService;
            _usersService = usersService;
            _gamesService = gamesService;
            _logger = logger;
        }
        
        public async Task<Game> StartGame(Guid userId, Guid adventureId)
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
            if (game == null)
                throw new Exception("user already has an instance of given adventure");

            throw new NotImplementedException();
        }
    }
}