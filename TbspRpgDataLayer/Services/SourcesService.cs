using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgSettings.Settings;

namespace TbspRpgDataLayer.Services
{
    public interface ISourcesService: IBaseService
    {
        Task<string> GetSourceTextForKey(Guid key, string language = null);
        Task<Source> GetSourceForKey(Guid key, Guid adventureId, string language);
        Task AddSource(Source source, string language = null);
        Task RemoveAllSourceForAdventure(Guid adventureId);
        Task RemoveSource(Guid sourceId);
        Task RemoveScriptFromSources(Guid scriptId);
        Task<List<Source>> GetAllSourceForAdventure(Guid adventureId, string language);
        Task<List<Source>> GetAllSourceAllLanguagesForAdventure(Guid adventureId);
        Task<Source> GetSourceById(Guid sourceId);
    }
    
    public class SourcesService : ISourcesService
    {
        private readonly ISourcesRepository _sourcesRepository;
        private readonly ILogger<SourcesService> _logger;

        public SourcesService(ISourcesRepository sourcesRepository,
            ILogger<SourcesService> logger)
        {
            _logger = logger;
            _sourcesRepository = sourcesRepository;
        }

        public Task<string> GetSourceTextForKey(Guid key, string language = null)
        {
            language ??= Languages.DEFAULT;
            return _sourcesRepository.GetSourceTextForKey(key, language);
        }

        public Task<Source> GetSourceForKey(Guid key, Guid adventureId, string language)
        {
            return _sourcesRepository.GetSourceForKey(key, adventureId, language);
        }
        
        public async Task AddSource(Source source, string language = null)
        {
            await _sourcesRepository.AddSource(source, language);
        }

        public async Task RemoveAllSourceForAdventure(Guid adventureId)
        {
            await _sourcesRepository.RemoveAllSourceForAdventure(adventureId);
        }

        public async Task RemoveSource(Guid sourceId)
        {
            await _sourcesRepository.RemoveSource(sourceId);
        }

        public async Task RemoveScriptFromSources(Guid scriptId)
        {
            var sources = await _sourcesRepository.GetSourcesWithScript(scriptId);
            foreach (var source in sources)
            {
                source.ScriptId = null;
            }
        }

        public Task<List<Source>> GetAllSourceForAdventure(Guid adventureId, string language)
        {
            return _sourcesRepository.GetAllSourceForAdventure(adventureId, language);
        }

        public Task<List<Source>> GetAllSourceAllLanguagesForAdventure(Guid adventureId)
        {
            return _sourcesRepository.GetAllSourceAllLanguagesForAdventure(adventureId);
        }

        public Task<Source> GetSourceById(Guid sourceId)
        {
            return _sourcesRepository.GetSourceById(sourceId);
        }

        public async Task SaveChanges()
        {
            await _sourcesRepository.SaveChanges();
        }
    }
}