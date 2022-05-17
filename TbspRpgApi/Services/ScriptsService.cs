using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services;

public interface IScriptsService
{
    Task<List<ScriptViewModel>> GetScriptsForAdventure(Guid adventureId);
}

public class ScriptsService: IScriptsService
{
    private readonly TbspRpgDataLayer.Services.IScriptsService _scriptsService;
    private readonly ILogger<ScriptsService> _logger;
    
    public ScriptsService(
        TbspRpgDataLayer.Services.IScriptsService scriptsService,
        ILogger<ScriptsService> logger)
    {
        _scriptsService = scriptsService;
        _logger = logger;
    }
    
    public async Task<List<ScriptViewModel>> GetScriptsForAdventure(Guid adventureId)
    {
        var scripts = await _scriptsService.GetScriptsForAdventure(adventureId);
        return scripts.Select(script => new ScriptViewModel(script)).ToList();
    }
}