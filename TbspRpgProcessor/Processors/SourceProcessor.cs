using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Services;

namespace TbspRpgProcessor.Processors
{
    public interface ISourceProcessor
    {
        Task<Source> CreateOrUpdateSource(Source updatedSource, string language);
    }
    
    public class SourceProcessor : ISourceProcessor
    {
        private readonly ISourcesService _sourcesService;
        private readonly ILogger<SourceProcessor> _logger;

        public SourceProcessor(ISourcesService sourcesService,
            ILogger<SourceProcessor> logger)
        {
            _sourcesService = sourcesService;
            _logger = logger;
        }

        public async Task<Source> CreateOrUpdateSource(Source updatedSource, string language)
        {
            if (updatedSource.Key == Guid.Empty)
            {
                // we need to create a new source object and save it
                var newSource = new Source()
                {
                    Key = Guid.NewGuid(),
                    AdventureId = updatedSource.AdventureId,
                    Name = updatedSource.Name,
                    Text = updatedSource.Text
                };
                await _sourcesService.AddSource(newSource);
                return newSource;
            }
            var dbSource = await _sourcesService.GetSourceForKey(updatedSource.Key, updatedSource.AdventureId, language);
            if(dbSource == null)
                throw new ArgumentException("invalid source key");
            dbSource.Text = updatedSource.Text;
            return dbSource;
        }
    }
}