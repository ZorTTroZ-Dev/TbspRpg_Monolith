using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer.Repositories;

public interface IScriptsRepository: IBaseRepository
{
    Task<Script> GetScriptById(Guid scriptId);
    Task<Script> GetScriptWithIncludedIn(Guid scriptId);
    Task<List<Script>> GetScriptsForAdventure(Guid adventureId);
    Task AddScript(Script script);
    void RemoveScript(Script script);
    void RemoveScripts(ICollection<Script> scripts);
    void AttachScript(Script script);
    Task<List<Script>> GetAdventureScriptsWithSourceReference(Guid adventureId, Guid sourceKey);
}

public class ScriptsRepository: IScriptsRepository
{
    private readonly DatabaseContext _databaseContext;

    public ScriptsRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<Script> GetScriptById(Guid scriptId)
    {
        return _databaseContext.Scripts.AsQueryable()
            .Include(script => script.Includes)
            .FirstOrDefaultAsync(script => script.Id == scriptId);
    }

    public Task<Script> GetScriptWithIncludedIn(Guid scriptId)
    {
        return _databaseContext.Scripts.AsQueryable()
            .Include(script => script.IncludedIn)
            .FirstOrDefaultAsync(script => script.Id == scriptId);
    }

    public Task<List<Script>> GetScriptsForAdventure(Guid adventureId)
    {
        return _databaseContext.Scripts.AsQueryable()
            .Include(script => script.Includes)
            .Where(script => script.AdventureId == adventureId)
            .ToListAsync();
    }

    public async Task AddScript(Script script)
    {
        await _databaseContext.AddAsync(script);
    }

    public void RemoveScript(Script script)
    {
        _databaseContext.Remove(script);
    }
    
    public void RemoveScripts(ICollection<Script> scripts)
    {
        _databaseContext.RemoveRange(scripts);
    }

    public void AttachScript(Script script)
    {
        _databaseContext.Attach(script);
    }

    public Task<List<Script>> GetAdventureScriptsWithSourceReference(Guid adventureId, Guid sourceKey)
    {
        return _databaseContext.Scripts.AsQueryable()
            .Where(script => script.AdventureId == adventureId && script.Content.Contains(sourceKey.ToString()))
            .ToListAsync();
    }

    public async Task SaveChanges()
    {
        await _databaseContext.SaveChangesAsync();
    }
}