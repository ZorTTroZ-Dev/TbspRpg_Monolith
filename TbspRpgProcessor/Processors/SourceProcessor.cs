using System;
using System.Collections.Generic;
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
        Task<Source> CreateOrUpdateSource(Source updatedSource, string language, bool saveChanges = false);
        Task<Source> GetSourceForKey(SourceForKeyModel sourceForKeyModel);
        Task<Guid> ResolveSourceKey(SourceForKeyModel sourceForKeyModel);
        Task<List<Source>> GetUnreferencedSources(UnreferencedSourceModel unreferencedSourceModel);
        Task RemoveSource(SourceRemoveModel sourceRemoveModel);
    }
    
    public class SourceProcessor : ISourceProcessor
    {
        private readonly IScriptProcessor _scriptProcessor;
        private readonly ISourcesService _sourcesService;
        private readonly IAdventuresService _adventuresService;
        private readonly ILocationsService _locationsService;
        private readonly IRoutesService _routesService;
        private readonly IContentsService _contentsService;
        private readonly IScriptsService _scriptsService;
        private readonly ILogger _logger;
        private readonly int MaxLoopCount = 5;

        public SourceProcessor(
            IScriptProcessor scriptProcessor,
            ISourcesService sourcesService,
            IAdventuresService adventuresService,
            ILocationsService locationsService,
            IRoutesService routesService,
            IContentsService contentsService,
            IScriptsService scriptsService,
            ILogger logger)
        {
            _scriptProcessor = scriptProcessor;
            _sourcesService = sourcesService;
            _adventuresService = adventuresService;
            _locationsService = locationsService;
            _routesService = routesService;
            _contentsService = contentsService;
            _scriptsService = scriptsService;
            _logger = logger;
        }

        public async Task<Source> CreateOrUpdateSource(Source updatedSource, string language, bool saveChanges = false)
        {
            if (updatedSource.Key == Guid.Empty)
            {
                // we need to create a new source object and save it
                var newSource = new Source()
                {
                    Key = Guid.NewGuid(),
                    AdventureId = updatedSource.AdventureId,
                    Name = updatedSource.Name,
                    Text = updatedSource.Text,
                    ScriptId = updatedSource.ScriptId
                };
                await _sourcesService.AddSource(newSource);
                if (saveChanges)
                    await _sourcesService.SaveChanges();
                return newSource;
            }
            var dbSource = await _sourcesService.GetSourceForKey(updatedSource.Key, updatedSource.AdventureId, language);
            if(dbSource == null)
                throw new ArgumentException("invalid source key");
            dbSource.Text = updatedSource.Text;
            dbSource.ScriptId = updatedSource.ScriptId;
            dbSource.Name = updatedSource.Name;
            if(saveChanges)
                await _sourcesService.SaveChanges();
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
                var result = await _scriptProcessor.ExecuteScript(new ScriptExecuteModel() {
                    ScriptId = dbSource.ScriptId.GetValueOrDefault(),
                    Game = sourceForKeyModel.Game
                });
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

        public async Task<List<Source>> GetUnreferencedSources(UnreferencedSourceModel unreferencedSourceModel)
        {
            // get all of the source entries for this adventure
            // go through each key
            // check if there is an adventure that has an InitialSourceKey or a DescriptionSourceKey equal to the key
            // check if there is content that has a SourceKey equal to the key
            // check if there is a location that has a SourceKey equal to the key
            // check if there is a route that has a SourceKey or a RouteTakenSourceKy equal to the key
            // check if there is a script with content that contains the key

            var sources = await _sourcesService.GetAllSourceAllLanguagesForAdventure(
                unreferencedSourceModel.AdventureId);
            for (var i = sources.Count - 1; i >= 0; i--)
            {
                var sourceKey = sources[i].Key;
                
                // check the adventure
                var adventureUseSource = await _adventuresService.DoesAdventureUseSource(
                    unreferencedSourceModel.AdventureId, sourceKey);

                // check the location
                var locationUseSource = await _locationsService.DoesAdventureLocationUseSource(
                    unreferencedSourceModel.AdventureId, sourceKey);
                
                // check the route
                var routeUseSource = await _routesService.DoesAdventureRouteUseSource(
                    unreferencedSourceModel.AdventureId, sourceKey);
                
                // check content
                var contentUseSource = await _contentsService.DoesAdventureContentUseSource(
                    unreferencedSourceModel.AdventureId, sourceKey);
                
                // check scripts
                var scriptsUseSource = await _scriptsService.IsSourceKeyReferenced(
                    unreferencedSourceModel.AdventureId, sourceKey);
                
                if(adventureUseSource
                   || locationUseSource
                   || routeUseSource
                   || contentUseSource
                   || scriptsUseSource)
                    sources.RemoveAt(i);
            }

            return sources;
        }

        public async Task RemoveSource(SourceRemoveModel sourceRemoveModel)
        {
            await _sourcesService.RemoveSource(sourceRemoveModel.SourceId);
            await _sourcesService.SaveChanges();
        }
    }
}