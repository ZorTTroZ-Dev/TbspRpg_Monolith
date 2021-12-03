using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface IGamesService : IBaseService
    {
        Task<Game> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId);
        Task AddGame(Game game);
        Task<Game> GetGameByIdIncludeLocation(Guid gameId);
        Task<Game> GetGameById(Guid gameId);
        Task<List<Game>> GetGamesByAdventureId(Guid adventureId);
        Task<List<Game>> GetGames(GameFilter filters);
        void RemoveGame(Game game);
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

        public Task<Game> GetGameById(Guid gameId)
        {
            return _gameRepository.GetGameById(gameId);
        }

        public Task<List<Game>> GetGamesByAdventureId(Guid adventureId)
        {
            return GetGames(new GameFilter()
            {
                AdventureId = adventureId
            });
        }

        public Task<List<Game>> GetGames(GameFilter filters)
        {
            return _gameRepository.GetGames(filters);
        }

        public void RemoveGame(Game game)
        {
            _gameRepository.RemoveGame(game);
        }

        public async Task SaveChanges()
        {
            await _gameRepository.SaveChanges();
        }
    }
}