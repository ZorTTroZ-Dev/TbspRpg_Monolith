using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Processors;

namespace TbspRpgApi.Services;

public interface IScriptsService
{
    Task<List<ScriptViewModel>> GetScriptsForAdventure(Guid adventureId);
    Task UpdateScript(ScriptUpdateRequest scriptUpdateRequest);
}

public class ScriptsService: IScriptsService
{
    private readonly IScriptProcessor _scriptProcessor;
    private readonly TbspRpgDataLayer.Services.IScriptsService _scriptsService;
    private readonly ILogger<ScriptsService> _logger;
    
    public ScriptsService(
        IScriptProcessor scriptProcessor,
        TbspRpgDataLayer.Services.IScriptsService scriptsService,
        ILogger<ScriptsService> logger)
    {
        _scriptProcessor = scriptProcessor;
        _scriptsService = scriptsService;
        _logger = logger;
    }
    
    public async Task<List<ScriptViewModel>> GetScriptsForAdventure(Guid adventureId)
    {
        var scripts = await _scriptsService.GetScriptsForAdventure(adventureId);
        return scripts.Select(script => new ScriptViewModel(script)).ToList();
    }

    public async Task UpdateScript(ScriptUpdateRequest scriptUpdateRequest)
    {
        await _scriptProcessor.UpdateScript(scriptUpdateRequest.ToScriptUpdateModel());
    }
}