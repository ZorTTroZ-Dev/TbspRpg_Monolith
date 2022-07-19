using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor;
using TbspRpgProcessor.Processors;

namespace TbspRpgApi.Services
{
    public interface IContentsService
    {
        Task<ContentViewModel> GetLatestForGame(Guid gameId);
        Task<ContentViewModel> GetPartialContentForGame(Guid gameId, ContentFilterRequest filterRequest);
        Task<ContentViewModel> GetContentForGameAfterPosition(Guid gameId, ulong position);
        Task<SourceViewModel> GetContentTextForKey(Guid gameId, Guid sourceKey);
        Task<SourceViewModel> GetProcessedContentTextForKey(Guid gameId, Guid sourceKey);
    }
    
    public class ContentsService : IContentsService
    {
        private readonly TbspRpgDataLayer.Services.IContentsService _contentsService;
        private readonly ITbspRpgProcessor _tbspRpgProcessor;
        private readonly ILogger<ContentsService> _logger;

        public ContentsService(
            ITbspRpgProcessor tbspRpgProcessor,
            TbspRpgDataLayer.Services.IContentsService contentsService,
            ILogger<ContentsService> logger)
        {
            _tbspRpgProcessor = tbspRpgProcessor;
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

        public async Task<ContentViewModel> GetContentForGameAfterPosition(Guid gameId, ulong position)
        {
            var contents = await _contentsService.GetContentForGameAfterPosition(gameId, position);
            return contents.Count > 0 ? new ContentViewModel(contents) : null;
        }
        
        public async Task<SourceViewModel> GetContentTextForKey(Guid gameId, Guid sourceKey)
        {
            var text = await _tbspRpgProcessor.GetContentTextForKey(gameId, sourceKey);
            return text == null ? null : new SourceViewModel(sourceKey, text);
        }
        
        public async Task<SourceViewModel> GetProcessedContentTextForKey(Guid gameId, Guid sourceKey)
        {
            var text = await _tbspRpgProcessor.GetContentTextForKey(gameId, sourceKey, true);
            return text == null ? null : new SourceViewModel(sourceKey, text);
        }
    }
}