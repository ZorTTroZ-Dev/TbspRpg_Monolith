using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface IGamesService : IBaseService
    {
        Task<Game> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId);
        Task AddGame(Game game);
        Task<Game> GetGameByIdIncludeLocation(Guid gameId);
    }
    
    public class GamesService : IGamesService
    {
        private readonly IGameRepository _gameRepository;
        private readonly ILogger<GamesService> _logger;
        
        public GamesService(IGameRepository gameRepository,
            ILogger<GamesService> logger)
        {
            _gameRepository = gameRepository;
            _logger = logger;
        }

        public Task<Game> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId)
        {
            return _gameRepository.GetGameByAdventureIdAndUserId(adventureId, userId);
        }

        public async Task AddGame(Game game)
        {
            await _gameRepository.AddGame(game);
        }

        public Task<Game> GetGameByIdIncludeLocation(Guid gameId)
        {
            return _gameRepository.GetGameByIdWithLocation(gameId);
        }

        public async Task SaveChanges()
        {
            await _gameRepository.SaveChanges();
        }
    }
}