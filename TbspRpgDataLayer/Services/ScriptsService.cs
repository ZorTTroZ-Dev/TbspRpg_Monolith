using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;

namespace TbspRpgDataLayer.Services;

public interface IScriptsService: IBaseService
{
    Task<Script> GetScriptById(Guid scriptId);
    Task<Script> GetScriptWithIncludedIn(Guid scriptId);
    Task<List<Script>> GetScriptsForAdventure(Guid adventureId);
    Task RemoveAllScriptsForAdventure(Guid adventureId);
    Task AddScript(Script script);
    void RemoveScript(Script script);
    void RemoveScripts(ICollection<Script> scripts);
    void AttachScript(Script script);
    Task<bool> IsSourceKeyReferenced(Guid adventureId, Guid sourceKey);
    Task<List<Script>> GetAdventureScriptsWithSourceReference(Guid adventureId, Guid sourceKey);
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

    public Task<Script> GetScriptWithIncludedIn(Guid scriptId)
    {
        return _scriptsRepository.GetScriptWithIncludedIn(scriptId);
    }

    public Task<List<Script>> GetScriptsForAdventure(Guid adventureId)
    {
        return _scriptsRepository.GetScriptsForAdventure(adventureId);
    }

    public async Task RemoveAllScriptsForAdventure(Guid adventureId)
    {
        var scripts = await _scriptsRepository.GetScriptsForAdventure(adventureId);
        _scriptsRepository.RemoveScripts(scripts);
    }

    public async Task AddScript(Script script)
    {
        await _scriptsRepository.AddScript(script);
    }

    public void RemoveScript(Script script)
    {
        _scriptsRepository.RemoveScript(script);
    }

    public void RemoveScripts(ICollection<Script> scripts)
    {
        _scriptsRepository.RemoveScripts(scripts);
    }

    public void AttachScript(Script script)
    {
        _scriptsRepository.AttachScript(script);
    }

    public async Task<bool> IsSourceKeyReferenced(Guid adventureId, Guid sourceKey)
    {
        var scripts = await GetAdventureScriptsWithSourceReference(adventureId, sourceKey);
        return scripts.Any();
    }

    public Task<List<Script>> GetAdventureScriptsWithSourceReference(Guid adventureId, Guid sourceKey)
    {
        return _scriptsRepository.GetAdventureScriptsWithSourceReference(adventureId, sourceKey);
    }
}