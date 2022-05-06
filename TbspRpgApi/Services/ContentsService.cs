using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
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
        private readonly IContentProcessor _contentProcessor;
        private readonly ILogger<ContentsService> _logger;

        public ContentsService(TbspRpgDataLayer.Services.IContentsService contentsService,
            IContentProcessor contentProcessor,
            ILogger<ContentsService> logger)
        {
            _contentsService = contentsService;
            _contentProcessor = contentProcessor;
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

        //TODO: Either get rid of these endpoints and have the frontend call the source endpoints
        // or have these use the source processor to get the actual text.
        public async Task<SourceViewModel> GetContentTextForKey(Guid gameId, Guid sourceKey)
        {
            var text = await _contentProcessor.GetContentTextForKey(gameId, sourceKey);
            return text == null ? null : new SourceViewModel(sourceKey, text);
        }
        
        public async Task<SourceViewModel> GetProcessedContentTextForKey(Guid gameId, Guid sourceKey)
        {
            var text = await _contentProcessor.GetContentTextForKey(gameId, sourceKey, true);
            return text == null ? null : new SourceViewModel(sourceKey, text);
        }
    }
}