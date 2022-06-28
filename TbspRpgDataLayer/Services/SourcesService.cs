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
    public interface ISourcesService
    {
        Task<string> GetSourceTextForKey(Guid key, string language = null);
        Task<Source> GetSourceForKey(Guid key, Guid adventureId, string language);
        Task AddSource(Source source, string language = null);
        Task RemoveAllSourceForAdventure(Guid adventureId);
        void RemoveScriptFromSources(Guid scriptId);
        Task<List<Source>> GetAllSourceForAdventure(Guid adventureId, string language);
        Task<List<Source>> GetAllSourceAllLanguagesForAdventure(Guid adventureId);
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

        public async void RemoveScriptFromSources(Guid scriptId)
        {
            var sources = await _sourcesRepository.GetSourcesWithScript(scriptId);
            foreach (var source in sources)
            {
                source.ScriptId = null;
            }
        }

        public async Task<List<Source>> GetAllSourceForAdventure(Guid adventureId, string language)
        {
            return await _sourcesRepository.GetAllSourceForAdventure(adventureId, language);
        }

        public async Task<List<Source>> GetAllSourceAllLanguagesForAdventure(Guid adventureId)
        {
            return await _sourcesRepository.GetAllSourceAllLanguagesForAdventure(adventureId);
        }
    }
}