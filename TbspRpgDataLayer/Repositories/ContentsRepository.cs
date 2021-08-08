using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface IContentsRepository : IBaseRepository
    {
        Task AddContent(Content content);
        Task<Content> GetContentForGameAtPosition(Guid gameId, ulong position);
    }
    
    public class ContentsRepository : IContentsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ContentsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task AddContent(Content content)
        {
            await _databaseContext.AddAsync(content);
        }

        public Task<Content> GetContentForGameAtPosition(Guid gameId, ulong position)
        {
            return _databaseContext.Contents.AsQueryable()
                .FirstOrDefaultAsync(content => content.GameId == gameId && content.Position == position);
        }

        public async Task SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}