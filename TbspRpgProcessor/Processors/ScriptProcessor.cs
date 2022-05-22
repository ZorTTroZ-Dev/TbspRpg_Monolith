using System;
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
    Task<string> ExecuteScript(Guid scriptId);
    Task UpdateScript(ScriptUpdateModel scriptUpdateModel);
}

public class ScriptProcessor : IScriptProcessor
{
    private readonly IScriptsService _scriptsService;
    private readonly ILogger<ScriptProcessor> _logger;

    public ScriptProcessor(IScriptsService scriptsService,
        ILogger<ScriptProcessor> logger)
    {
        _scriptsService = scriptsService;
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
    public async Task<string> ExecuteScript(Guid scriptId)
    {
        if(scriptId == Guid.Empty)
            throw new ArgumentException("invalid script id");
            
        var script = await _scriptsService.GetScriptById(scriptId);
        if (script == null)
            throw new ArgumentException("invalid script id");
        
        var luaState = new Lua();
        
        // load sandbox lua library
        luaState["sandbox"] = luaState.DoString(LuaSandbox.LuaSandboxCode).First();
        
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
                Includes = scriptUpdateModel.script.Includes
            };
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