using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.Services;

public interface IScriptsService
{
    Task<List<ScriptViewModel>> GetScriptsForAdventure(Guid adventureId);
    Task UpdateScript(ScriptUpdateRequest scriptUpdateRequest);
    Task DeleteScript(Guid scriptId);
}

public class ScriptsService: IScriptsService
{
    private readonly ITbspRpgProcessor _tbspRpgProcessor;
    private readonly TbspRpgDataLayer.Services.IScriptsService _scriptsService;
    private readonly ILogger<ScriptsService> _logger;
    
    public ScriptsService(
        ITbspRpgProcessor tbspRpgProcessor,
        TbspRpgDataLayer.Services.IScriptsService scriptsService,
        ILogger<ScriptsService> logger)
    {
        _tbspRpgProcessor = tbspRpgProcessor;
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
        await _tbspRpgProcessor.UpdateScript(scriptUpdateRequest.ToScriptUpdateModel());
    }

    public async Task DeleteScript(Guid scriptId)
    {
        await _tbspRpgProcessor.RemoveScript(new ScriptRemoveModel()
        {
            ScriptId = scriptId
        });
    }
}