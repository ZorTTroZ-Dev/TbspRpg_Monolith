using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface IGameRepository : IBaseRepository
    {
        Task<Game> GetGameById(Guid gameId);
        Task<Game> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId);
        void AddGame(Game game);
    }
    
    public class GameRepository : IGameRepository
    {
        private readonly DatabaseContext _databaseContext;

        public GameRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Task<Game> GetGameById(Guid gameId)
        {
            return _databaseContext.Games.AsQueryable().FirstOrDefaultAsync(g => g.Id == gameId);
        }

        public Task<Game> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId)
        {
            return _databaseContext.Games.AsQueryable()
                .FirstOrDefaultAsync(g => g.AdventureId == adventureId && g.UserId == userId);
        }

        public async void AddGame(Game game)
        {
            await _databaseContext.Games.AddAsync(game);
        }

        public async void SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}