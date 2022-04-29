using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NLua;
using TbspRpgDataLayer.Services;
using TbspRpgSettings.Settings;

namespace TbspRpgProcessor.Processors;

public interface IScriptProcessor
{
    // a script can only return a GUID which is a source id
    Task<string> ExecuteScript(Guid scriptId);
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
    // return the result of the script?
        // how am I going to return the result, just return a string and let the caller
        // interpret it how they want?, add a return type to the script object?
        
    // I've also been thinking of adding variables to the content that can be replaced in the
    // associated source. So when content added, provide a source key and variable values that
    // would get replaced when source rendered.
    public async Task<string> ExecuteScript(Guid scriptId)
    {
        var script = await _scriptsService.GetScriptById(scriptId);
        if (script == null)
            throw new ArgumentException("invalid script id");
        
        var luaState = new Lua();
        
        // load sandbox lua library
        luaState["sandbox"] = luaState.DoString(LuaSandbox.LuaSandboxCode).First();
        
        // load the script
        luaState.DoString(script.Content);
        
        // have to put the run function in the environment or it won't run
        luaState.DoString("sandboxed_run = sandbox('run()', {env = { run = run }})");
        
        // run the script
        var scriptFunc = luaState["sandboxed_run"] as LuaFunction;
        if (scriptFunc == null) return null;
        scriptFunc.Call();
        return luaState["result"] as string;
    }
}