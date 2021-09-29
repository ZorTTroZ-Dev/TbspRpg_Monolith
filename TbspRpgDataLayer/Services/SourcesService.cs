using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgSettings.Settings;

namespace TbspRpgDataLayer.Services
{
    public interface ISourcesService
    {
        Task<string> GetSourceTextForKey(Guid key, string language = null);
        Task<Source> GetSourceForKey(Guid key, Guid adventureId, string language);
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
    }
}