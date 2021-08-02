using System;
using System.Threading.Tasks;
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

        public GameProcessor(
            IAdventuresService adventuresService,
            IUsersService usersService,
            IGamesService gamesService)
        {
            _adventuresService = adventuresService;
            _usersService = usersService;
            _gamesService = gamesService;
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