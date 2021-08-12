using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services
{
    public interface IContentsService
    {
        Task<ContentViewModel> GetLatestForGame(Guid gameId);
        Task<ContentViewModel> GetPartialContentForGame(Guid gameId, ContentFilterRequest filterRequest);
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

        public async Task<ContentViewModel> GetPartialContentForGame(Guid gameId, ContentFilterRequest filterRequest)
        {
            var contents = await _contentsService.GetPartialContentForGame(gameId,
                new TbspRpgDataLayer.ArgumentModels.ContentFilterRequest()
                {
                    Direction = filterRequest.Direction,
                    Start = filterRequest.Start,
                    Count = filterRequest.Count
                });
            return contents.Count > 0 ? new ContentViewModel(contents) : null;
        }
    }
}