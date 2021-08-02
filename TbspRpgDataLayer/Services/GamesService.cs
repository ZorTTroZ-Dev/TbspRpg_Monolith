using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface IGamesService
    {
        Task<Game> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId);
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
    }
}