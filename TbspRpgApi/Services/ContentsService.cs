using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services
{
    public interface IContentsService
    {
        Task<ContentViewModel> GetLatestForGame(Guid gameId);
    }
    
    public class ContentsService : IContentsService
    {
        private readonly TbspRpgDataLayer.Services.IContentsService _contentsService;
        private readonly ILogger<ContentsService> _logger;

        public ContentsService(TbspRpgDataLayer.Services.IContentsService contentsService,
            ILogger<ContentsService> logger)
        {
            _contentsService = contentsService;
            _logger = logger;
        }

        public async Task<ContentViewModel> GetLatestForGame(Guid gameId)
        {
            var content = await _contentsService.GetLatestForGame(gameId);
            return content != null ? new ContentViewModel(content) : null;
        }
    }
}