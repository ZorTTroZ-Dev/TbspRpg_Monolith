using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services
{
    public interface IContentsService : IBaseService
    {
        Task AddContent(Content content);
        Task<Content> GetContentForGameAtPosition(Guid gameId, ulong position);
    }
    
    public class ContentsService : IContentsService
    {
        private readonly IContentsRepository _contentsRepository;
        private readonly ILogger<ContentsService> _logger;

        public ContentsService(IContentsRepository contentsRepository,
            ILogger<ContentsService> logger)
        {
            _contentsRepository = contentsRepository;
            _logger = logger;
        }
        
        public async Task SaveChanges()
        {
            await _contentsRepository.SaveChanges();
        }

        public async Task AddContent(Content content)
        {
            var dbContent = await GetContentForGameAtPosition(content.GameId, content.Position);
            if(dbContent == null)
                await _contentsRepository.AddContent(content);
        }

        public Task<Content> GetContentForGameAtPosition(Guid gameId, ulong position)
        {
            return _contentsRepository.GetContentForGameAtPosition(gameId, position);
        }
    }
}