using System;
using System.Threading.Tasks;
using Markdig;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface ISourceProcessor
    {
        Task<Source> CreateOrUpdateSource(Source updatedSource, string language);
        Task<Source> GetSourceForKey(SourceForKeyModel sourceForKeyModel);
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

        public async Task<Source> GetSourceForKey(SourceForKeyModel sourceForKeyModel)
        {
            var dbSource = await _sourcesService.GetSourceForKey(
                sourceForKeyModel.Key, sourceForKeyModel.AdventureId, sourceForKeyModel.Language);

            if (dbSource != null)
            {
                dbSource.Text = Markdown.ToHtml(dbSource.Text).Trim();
            }

            return dbSource;
        }
    }
}