using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services;

public interface IScriptsService: IBaseService
{
    Task<Script> GetScriptById(Guid scriptId);
    Task<List<Script>> GetScriptsForAdventure(Guid adventureId);
}

public class ScriptsService: IScriptsService
{
    private readonly IScriptsRepository _scriptsRepository;
    private readonly ILogger<ScriptsService> _logger;

    public ScriptsService(IScriptsRepository scriptsRepository,
        ILogger<ScriptsService> logger)
    {
        _scriptsRepository = scriptsRepository;
        _logger = logger;
    }
    
    public async Task SaveChanges()
    {
        await _scriptsRepository.SaveChanges();
    }

    public Task<Script> GetScriptById(Guid scriptId)
    {
        return _scriptsRepository.GetScriptById(scriptId);
    }

    public Task<List<Script>> GetScriptsForAdventure(Guid adventureId)
    {
        return _scriptsRepository.GetScriptsForAdventure(adventureId);
    }
}