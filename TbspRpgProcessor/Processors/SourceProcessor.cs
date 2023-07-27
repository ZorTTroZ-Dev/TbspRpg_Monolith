using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Markdig;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;
using TbspRpgSettings;
using TbspRpgSettings.Settings;

namespace TbspRpgProcessor.Processors
{
    public interface ISourceProcessor
    {
        Task<Source> CreateOrUpdateSource(SourceCreateOrUpdateModel sourceCreateOrUpdateModel);
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
        private readonly TbspRpgUtilities _tbspRpgUtilities;
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
            TbspRpgUtilities tbspRpgUtilities,
            ILogger logger)
        {
            _scriptProcessor = scriptProcessor;
            _sourcesService = sourcesService;
            _adventuresService = adventuresService;
            _locationsService = locationsService;
            _routesService = routesService;
            _contentsService = contentsService;
            _scriptsService = scriptsService;
            _tbspRpgUtilities = tbspRpgUtilities;
            _logger = logger;
        }

        public async Task<Source> CreateOrUpdateSource(SourceCreateOrUpdateModel sourceCreateOrUpdateModel)
        {
            if (sourceCreateOrUpdateModel.Source.Key == Guid.Empty)
            {
                // we need to create a new source object and save it
                var newSource = new Source()
                {
                    Key = Guid.NewGuid(),
                    AdventureId = sourceCreateOrUpdateModel.Source.AdventureId,
                    Name = sourceCreateOrUpdateModel.Source.Name,
                    Text = sourceCreateOrUpdateModel.Source.Text,
                    ScriptId = Guid.Empty
                };
                await _sourcesService.AddSource(newSource);
                await CompileSourceScript(newSource, false);
                if (sourceCreateOrUpdateModel.Save)
                    await _sourcesService.SaveChanges();
                return newSource;
            }
            var dbSource = await _sourcesService.GetSourceForKey(
                sourceCreateOrUpdateModel.Source.Key,
                sourceCreateOrUpdateModel.Source.AdventureId,
                sourceCreateOrUpdateModel.Language);
            if(dbSource == null)
                throw new ArgumentException("invalid source key");
            dbSource.Text = sourceCreateOrUpdateModel.Source.Text;
            dbSource.Name = sourceCreateOrUpdateModel.Source.Name;
            await RecompileSourceScript(dbSource);
            if(sourceCreateOrUpdateModel.Save)
                await _sourcesService.SaveChanges();
            return dbSource;
        }

        private async Task RecompileSourceScript(Source source)
        {
            // the source currently doesn't have a script but they may have added some
            if (source.ScriptId == null || source.ScriptId == Guid.Empty)
            {
                await CompileSourceScript(source, false);
                return;
            }
            
            // the source currently has a script, check if it still does, if so update it
            var dbScript = await _scriptsService.GetScriptById(source.ScriptId.GetValueOrDefault());
            if (dbScript == null)
            {
                throw new ArgumentException("invalid script id");
            }

            // check if there are any script sections
            if (!_tbspRpgUtilities.EmbeddedSourceScriptRegex.IsMatch(source.Text))
            {
                // remove the script from the database, the script blocks from the source was removed
                _scriptsService.RemoveScript(dbScript);
                source.ScriptId = Guid.Empty;
            }
            else
            {
                dbScript.Content = GenerateSourceScript(source.Text);
            }
        }

        private string GenerateSourceScript(string sourceContent)
        {
            // create a function for each embedded chunk of script
            var scriptContent = "";
            var functionCount = 0;
            foreach (Match scriptChunk in _tbspRpgUtilities.EmbeddedSourceScriptRegex.Matches(sourceContent))
            {
                var chunkContent = scriptChunk.Groups[1].Value.Trim();
                scriptContent += $"\nfunction func{functionCount}()\n{chunkContent}\nend";
                functionCount++;
            }
            
            // generate a run function that calls each previously generated function
            // take the result of each function and return as semicolon seperated string
            scriptContent += "\nfunction run()";
            var callFunctions = "\n\t";
            var setResult = "\n\tresult = ";
            for (var i = 0; i < functionCount; i++)
            {
                callFunctions += $"result{i} = func{i}()\n\t";
                if (i != 0)
                    setResult += " .. ';' .. ";
                setResult += $"result{i}";
            }

            scriptContent += callFunctions;
            scriptContent += setResult;
            scriptContent += "\nend";
            return scriptContent;
        }

        private async Task<Script> CompileSourceScript(Source source, bool save = true)
        {
            // check if there are any script sections
            if (!_tbspRpgUtilities.EmbeddedSourceScriptRegex.IsMatch(source.Text))
            {
                return null;
            }
            
            // save the script to the database for this source object
            var script = await _scriptProcessor.CreateScript(new ScriptCreateModel()
            {
                script = new Script()
                {
                    Id = Guid.Empty,
                    AdventureId = source.AdventureId,
                    Name = source.Name + "_script",
                    Type = ScriptTypes.LuaScript,
                    Content = GenerateSourceScript(source.Text),
                    Includes = new List<Script>()
                },
                Save = false
            });

            source.ScriptId = script.Id;
            if(save)
                await _sourcesService.SaveChanges();
            return script;
        }

        private async Task ReplaceEmbeddedScript(Source source, Game game)
        {
            // check if there are any script sections
            if (!_tbspRpgUtilities.EmbeddedSourceScriptRegex.IsMatch(source.Text))
            {
                return;
            }

            Script script = null;
            if (source.ScriptId == null || source.ScriptId == Guid.Empty)
            {
                script = await CompileSourceScript(source);
            }
            else
            {
                script = await _scriptsService.GetScriptById(source.ScriptId.GetValueOrDefault());
            }
            if (script == null)
            {
                throw new Exception("source has invalid script id");
            }

            // execute the script
            var result = await _scriptProcessor.ExecuteScript(new ScriptExecuteModel()
            {
                Script = script,
                Game = game
            });
            var splitResult = result.Split(';');
            if (_tbspRpgUtilities.EmbeddedSourceScriptRegex.Matches(source.Text).Count <= splitResult.Length)
            {
                var matchIndex = 0;
                source.Text = _tbspRpgUtilities.EmbeddedSourceScriptRegex.Replace(
                    source.Text, m => splitResult[matchIndex++]);
            }
            else
            {
                throw new Exception("source script bad result");
            }
        }

        public async Task<Source> GetSourceForKey(SourceForKeyModel sourceForKeyModel)
        {
            var dbSource = await _sourcesService.GetSourceForKey(
                sourceForKeyModel.Key,
                sourceForKeyModel.AdventureId,
                sourceForKeyModel.Language);

            if (sourceForKeyModel.Processed && dbSource != null)
            {
                await ReplaceEmbeddedScript(dbSource, sourceForKeyModel.Game);
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