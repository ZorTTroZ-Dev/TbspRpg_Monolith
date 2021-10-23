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
        Task AddSource(Source source, string language = null);
        Task<Source> CreateOrUpdateSource(Source source, string language);
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

        public async Task<Source> CreateOrUpdateSource(Source source, string language)
        {
            if (source.Key == Guid.Empty)
            {
                // we need to create a new source object and save it
                var newSource = new Source()
                {
                    Key = Guid.NewGuid(),
                    AdventureId = source.AdventureId,
                    Name = source.Name,
                    Text = source.Text
                };
                await AddSource(newSource);
                return newSource;
            }
            var dbSource = await GetSourceForKey(source.Key, source.AdventureId, language);
            if(dbSource == null)
                throw new ArgumentException("invalid source key");
            dbSource.Text = source.Text;
            return dbSource;
        }
    }
}