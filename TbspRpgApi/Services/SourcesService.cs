using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.Services
{
    public interface ISourcesService
    {
        public Task<SourceViewModel> GetSourceForKey(Guid key, Guid adventureId, string language);
        public Task<SourceViewModel> GetProcessedSourceForKey(Guid key, Guid adventureId, string language);
        public Task<List<SourceViewModel>> GetSourcesForAdventure(Guid adventureId, string language);
        public Task<List<SourceViewModel>> GetSourceAllLanguagesForAdventure(Guid adventureId);
        Task UpdateSource(SourceUpdateRequest sourceUpdateRequest);
        Task<List<SourceViewModel>> GetUnreferencedSourcesForAdventure(Guid adventureId);
        Task DeleteSource(Guid sourceId);
    }
    
    public class SourcesService: ISourcesService
    {
        private readonly TbspRpgDataLayer.Services.ISourcesService _sourcesService;
        private readonly ITbspRpgProcessor _tbspRpgProcessor;
        private readonly ILogger<SourcesService> _logger;

        public SourcesService(TbspRpgDataLayer.Services.ISourcesService sourcesService,
            ITbspRpgProcessor tbspRpgProcessor,
            ILogger<SourcesService> logger)
        {
            _sourcesService = sourcesService;
            _tbspRpgProcessor = tbspRpgProcessor;
            _logger = logger;
        }

        public async Task<SourceViewModel> GetSourceForKey(Guid key, Guid adventureId, string language)
        {
            var source = await _sourcesService.GetSourceForKey(key, adventureId, language);
            return source != null ? new SourceViewModel(source, language) : null;
        }
        
        public async Task<SourceViewModel> GetProcessedSourceForKey(Guid key, Guid adventureId, string language)
        {
            var source = await _tbspRpgProcessor.GetSourceForKey(new SourceForKeyModel()
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

        public async Task UpdateSource(SourceUpdateRequest sourceUpdateRequest)
        {
            await _tbspRpgProcessor.CreateOrUpdateSource(new SourceCreateOrUpdateModel() {
                Source = sourceUpdateRequest.Source.ToEntity(),
                Language = sourceUpdateRequest.Source.Language,
                Save = true
            });
        }

        public async Task<List<SourceViewModel>> GetUnreferencedSourcesForAdventure(Guid adventureId)
        {
            var sources = await _tbspRpgProcessor.GetUnreferencedSources(new UnreferencedSourceModel()
            {
                AdventureId = adventureId
            });
            return sources.Select(source => new SourceViewModel(source)).ToList();
        }

        public async Task DeleteSource(Guid sourceId)
        {
            await _tbspRpgProcessor.RemoveSource(new SourceRemoveModel()
            {
                SourceId = sourceId
            });
        }
    }
}