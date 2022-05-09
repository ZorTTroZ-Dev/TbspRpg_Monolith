using System;
using System.Threading.Tasks;
using Markdig;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface ISourceProcessor
    {
        Task<Source> CreateOrUpdateSource(Source updatedSource, string language);
        Task<Source> GetSourceForKey(SourceForKeyModel sourceForKeyModel);
        Task<Guid> ResolveSourceKey(SourceForKeyModel sourceForKeyModel);
    }
    
    public class SourceProcessor : ISourceProcessor
    {
        private readonly IScriptProcessor _scriptProcessor;
        private readonly ISourcesService _sourcesService;
        private readonly ILogger<SourceProcessor> _logger;
        private readonly int MaxLoopCount = 5;

        public SourceProcessor(
            IScriptProcessor scriptProcessor,
            ISourcesService sourcesService,
            ILogger<SourceProcessor> logger)
        {
            _scriptProcessor = scriptProcessor;
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
                sourceForKeyModel.Key,
                sourceForKeyModel.AdventureId,
                sourceForKeyModel.Language);

            if (sourceForKeyModel.Processed && dbSource != null)
            {
                dbSource.Text = Markdown.ToHtml(dbSource.Text).Trim();
            }

            return dbSource;
        }

        public async Task<Guid> ResolveSourceKey(SourceForKeyModel sourceForKeyModel)
        {
            // load the source for the given key
            // while source is a script
            // call script, get result, load source
            var dbSource = await _sourcesService.GetSourceForKey(
                sourceForKeyModel.Key,
                sourceForKeyModel.AdventureId,
                sourceForKeyModel.Language);

            if (dbSource == null)
            {
                throw new ArgumentException("invalid source key");
            }

            var loopCount = 0;
            while (dbSource.ScriptId != null && loopCount < MaxLoopCount)
            {
                var result = await _scriptProcessor.ExecuteScript(dbSource.ScriptId.GetValueOrDefault());
                var sourceKey = Guid.Parse(result);
                dbSource = await _sourcesService.GetSourceForKey(
                    sourceKey,
                    sourceForKeyModel.AdventureId,
                    sourceForKeyModel.Language);
                if (dbSource == null)
                {
                    throw new ArgumentException("invalid source key");
                }

                loopCount++;
            }

            if (loopCount >= MaxLoopCount)
            {
                throw new Exception("source never resolved");
            }

            return dbSource.Key;
        }
    }
}