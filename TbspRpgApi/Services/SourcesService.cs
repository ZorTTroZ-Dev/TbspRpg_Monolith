using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services
{
    public interface ISourcesService
    {
        public Task<SourceViewModel> GetSourceForKey(Guid key, Guid adventureId, string language);
    }
    
    public class SourcesService: ISourcesService
    {
        private readonly TbspRpgDataLayer.Services.ISourcesService _sourcesService;
        private readonly ILogger<SourcesService> _logger;

        public SourcesService(TbspRpgDataLayer.Services.ISourcesService sourcesService,
            ILogger<SourcesService> logger)
        {
            _sourcesService = sourcesService;
            _logger = logger;
        }

        public async Task<SourceViewModel> GetSourceForKey(Guid key, Guid adventureId, string language)
        {
            var source = await _sourcesService.GetSourceForKey(key, adventureId, language);
            return source != null ? new SourceViewModel(source) : null;
        }
    }
}