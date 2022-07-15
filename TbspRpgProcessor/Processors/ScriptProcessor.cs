using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLua;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;

namespace TbspRpgProcessor.Processors;

public interface IScriptProcessor
{
    Task<string> ExecuteScript(Guid scriptId, Guid gameId);
    Task<string> ExecuteScript(Guid scriptId, Game game);
    Task<string> ExecuteScript(Guid scriptId);
    string ExecuteScript(Script script, Game game);
    Task RemoveScript(ScriptRemoveModel scriptIdRemoveModel);
    Task UpdateScript(ScriptUpdateModel scriptUpdateModel);
}

public class ScriptProcessor : IScriptProcessor
{
    private readonly IScriptsService _scriptsService;
    private readonly IAdventuresService _adventuresService;
    private readonly IRoutesService _routesService;
    private readonly ILocationsService _locationsService;
    private readonly ISourcesService _sourcesService;
    private readonly IGamesService _gamesService;
    private readonly ILogger _logger;

    public ScriptProcessor(IScriptsService scriptsService,
        IAdventuresService adventuresService,
        IRoutesService routesService,
        ILocationsService locationsService,
        ISourcesService sourcesService,
        IGamesService gamesService,
        ILogger logger)
    {
        _scriptsService = scriptsService;
        _adventuresService = adventuresService;
        _routesService = routesService;
        _locationsService = locationsService;
        _sourcesService = sourcesService;
        _gamesService = gamesService;
        _logger = logger;
    }
    
    // Execute Script Method
    // Crete the Lua environment
    // Sandbox the environment, https://github.com/kikito/lua-sandbox
    // load the game state from the database, future addition
    // Load any scripts included by the script
    // add the script to the environment
    // execute the script
    // update the game state in the database, future addition
    // retrieve the result of the script
        
    // I've also been thinking of adding variables to the content that can be replaced in the
    // associated source. So when content added, provide a source key and variable values that
    // would get replaced when source rendered.
    private async Task<Script> VerifyScriptId(Guid scriptId)
    {
        if(scriptId == Guid.Empty)
            throw new ArgumentException("invalid script id");
            
        var dbScript = await _scriptsService.GetScriptById(scriptId);
        if (dbScript == null)
            throw new ArgumentException("invalid script id");
        
        return dbScript;
    }
    
    public async Task<string> ExecuteScript(Guid scriptId, Guid gameId)
    {
        var dbScript = await VerifyScriptId(scriptId);
        
        var dbGame = await _gamesService.GetGameById(gameId);
        if (dbGame == null)
            throw new ArgumentException("invalid game id");
        
        return ExecuteScript(dbScript, dbGame);
    }
    
    public async Task<string> ExecuteScript(Guid scriptId, Game game)
    {
        var dbScript = await VerifyScriptId(scriptId);
        return ExecuteScript(dbScript, game);
    }
    
    public async Task<string> ExecuteScript(Guid scriptId)
    {
        var dbScript = await VerifyScriptId(scriptId);
        return ExecuteScript(dbScript, null);
    }

    public string ExecuteScript(Script script, Game game)
    {
        var luaState = new Lua();
        
        // load sandbox lua library
        luaState["sandbox"] = luaState.DoString(LuaSandbox.LuaSandboxCode).First();
        
        // add the game as a global variable if it exists
        if (game != null)
        {
            luaState["game"] = game;
        }
        
        // load any includes
        if (script.Includes != null)
        {
            foreach (var include in script.Includes)
            {
                luaState.DoString(include.Content);
            }
        }

        // load the script
        luaState.DoString(script.Content);
        
        // have to put the run function in the environment or it won't run
        luaState.DoString("sandbox_run = sandbox('run()', {env = { run = run }})");
        
        // run the script
        var scriptFunc = luaState["sandbox_run"] as LuaFunction;
        if (scriptFunc == null) return null;
        scriptFunc.Call();
        return luaState["result"] as string;
    }

    public async Task RemoveScript(ScriptRemoveModel scriptRemoveModel)
    {
        var dbScript = await _scriptsService.GetScriptWithIncludedIn(scriptRemoveModel.ScriptId);
        if (dbScript == null)
        {
            throw new ArgumentException("invalid script id");
        }
        
        // need to check if script attached to an adventure, location, route or source
        _adventuresService.RemoveScriptFromAdventures(scriptRemoveModel.ScriptId);
        _routesService.RemoveScriptFromRoutes(scriptRemoveModel.ScriptId);
        _locationsService.RemoveScriptFromLocations(scriptRemoveModel.ScriptId);
        _sourcesService.RemoveScriptFromSources(scriptRemoveModel.ScriptId);
        
        // remove this script from being included in other scripts
        dbScript.IncludedIn = new List<Script>();
        
        // then delete the script
        _scriptsService.RemoveScript(dbScript);

        await _scriptsService.SaveChanges();
    }

    public async Task UpdateScript(ScriptUpdateModel scriptUpdateModel)
    {
        if (scriptUpdateModel.script.Id == Guid.Empty)
        {
            // create a new script
            var script = new Script()
            {
                Id = Guid.NewGuid(),
                AdventureId = scriptUpdateModel.script.AdventureId,
                Name = scriptUpdateModel.script.Name,
                Type = scriptUpdateModel.script.Type,
                Content = scriptUpdateModel.script.Content,
                Includes = new List<Script>()
            };
            foreach (var include in scriptUpdateModel.script.Includes)
            {
                _scriptsService.AttachScript(include);
                script.Includes.Add(include);
            }
            await _scriptsService.AddScript(script);
        }
        else
        {
            // update existing script
            var dbScript = await _scriptsService.GetScriptById(scriptUpdateModel.script.Id);
            if (dbScript == null)
                throw new ArgumentException("invalid script id");
            dbScript.Name = scriptUpdateModel.script.Name;
            dbScript.Type = scriptUpdateModel.script.Type;
            dbScript.Content = scriptUpdateModel.script.Content;
            dbScript.Includes = scriptUpdateModel.script.Includes;
        }

        await _scriptsService.SaveChanges();
    }
}