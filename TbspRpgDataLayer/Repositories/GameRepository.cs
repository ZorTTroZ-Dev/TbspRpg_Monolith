using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface IGameRepository : IBaseRepository
    {
        Task<Game> GetGameById(Guid gameId);
        Task<Game> GetGameByIdWithLocation(Guid gameId);
        Task<Game> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId);
        Task AddGame(Game game);
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

        public Task<Game> GetGameByIdWithLocation(Guid gameId)
        {
            return _databaseContext.Games.AsQueryable().Include(g => g.Location)
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }

        public Task<Game> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId)
        {
            return _databaseContext.Games.AsQueryable()
                .FirstOrDefaultAsync(g => g.AdventureId == adventureId && g.UserId == userId);
        }

        public async Task AddGame(Game game)
        {
            await _databaseContext.Games.AddAsync(game);
        }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}