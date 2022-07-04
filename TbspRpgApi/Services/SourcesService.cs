using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;
using TbspRpgProcessor.Processors;

namespace TbspRpgApi.Services
{
    public interface ISourcesService
    {
        public Task<SourceViewModel> GetSourceForKey(Guid key, Guid adventureId, string language);
        public Task<SourceViewModel> GetProcessedSourceForKey(Guid key, Guid adventureId, string language);
        public Task<List<SourceViewModel>> GetSourcesForAdventure(Guid adventureId, string language);
        public Task<List<SourceViewModel>> GetSourceAllLanguagesForAdventure(Guid adventureId);
    }
    
    public class SourcesService: ISourcesService
    {
        private readonly TbspRpgDataLayer.Services.ISourcesService _sourcesService;
        private readonly ISourceProcessor _sourceProcessor;
        private readonly ILogger<SourcesService> _logger;

        public SourcesService(TbspRpgDataLayer.Services.ISourcesService sourcesService,
            ISourceProcessor sourceProcessor,
            ILogger<SourcesService> logger)
        {
            _sourcesService = sourcesService;
            _sourceProcessor = sourceProcessor;
            _logger = logger;
        }

        public async Task<SourceViewModel> GetSourceForKey(Guid key, Guid adventureId, string language)
        {
            var source = await _sourcesService.GetSourceForKey(key, adventureId, language);
            return source != null ? new SourceViewModel(source, language) : null;
        }
        
        public async Task<SourceViewModel> GetProcessedSourceForKey(Guid key, Guid adventureId, string language)
        {
            var source = await _sourceProcessor.GetSourceForKey(new SourceForKeyModel()
            {
                Key = key,
                AdventureId = adventureId,
                Language = language,
                Processed = true
            });
            return source != null ? new SourceViewModel(source, language) : null;
        }

        public async Task<List<SourceViewModel>> GetSourcesForAdventure(Guid adventureId, string language)
        {
            var sources = await _sourcesService.GetAllSourceForAdventure(adventureId, language);
            return sources.Select(source => new SourceViewModel(source, language)).ToList();
        }

        public async Task<List<SourceViewModel>> GetSourceAllLanguagesForAdventure(Guid adventureId)
        {
            var sources = await _sourcesService.GetAllSourceAllLanguagesForAdventure(adventureId);
            return sources.Select(source => new SourceViewModel(source)).ToList();
        }
    }
}