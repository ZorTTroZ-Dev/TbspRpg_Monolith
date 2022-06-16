using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface IGameRepository : IBaseRepository
    {
        Task<Game> GetGameById(Guid gameId);
        Task<Game> GetGameByIdWithAdventure(Guid gameId);
        Task<Game> GetGameByIdWithLocation(Guid gameId);
        Task<Game> GetGameByAdventureIdAndUserId(Guid adventureId, Guid userId);
        Task<List<Game>> GetGames(GameFilter filters);
        Task<List<Game>> GetGamesIncludeUsers(GameFilter filters);
        Task AddGame(Game game);
        void RemoveGame(Game game);
        void RemoveGames(ICollection<Game> games);
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
            return _databaseContext.Games.AsQueryable()
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }
        
        public Task<Game> GetGameByIdWithAdventure(Guid gameId)
        {
            return _databaseContext.Games.AsQueryable()
                .Include(g => g.Adventure)
                .FirstOrDefaultAsync(g => g.Id == gameId);
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

        private IQueryable<Game> BuildGamesQuery(GameFilter filters)
        {
            var query = _databaseContext.Games.AsQueryable();
            if (filters.AdventureId != Guid.Empty)
                query = query.Where(game => game.AdventureId == filters.AdventureId);
            if (filters.UserId != Guid.Empty)
                query = query.Where(game => game.UserId == filters.UserId);
            return query;
        }

        public Task<List<Game>> GetGames(GameFilter filters)
        {
            if (filters == null) return _databaseContext.Games.AsQueryable().ToListAsync();
            var query = BuildGamesQuery(filters);
            return query.ToListAsync();
        }

        public Task<List<Game>> GetGamesIncludeUsers(GameFilter filters)
        {
            if (filters == null) return _databaseContext.Games.AsQueryable().Include(g => g.User).ToListAsync();
            var query = BuildGamesQuery(filters);
            query = query.Include(g => g.User);
            return query.ToListAsync();
        }

        public async Task AddGame(Game game)
        {
            await _databaseContext.Games.AddAsync(game);
        }

        public void RemoveGame(Game game)
        {
            _databaseContext.Remove(game);
        }

        public void RemoveGames(ICollection<Game> games)
        {
            _databaseContext.RemoveRange(games);
        }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}