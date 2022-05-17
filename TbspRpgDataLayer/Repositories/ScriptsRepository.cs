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
    Task<List<Script>> GetScriptsForAdventure(Guid adventureId);
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

    public Task<List<Script>> GetScriptsForAdventure(Guid adventureId)
    {
        return _databaseContext.Scripts.AsQueryable()
            .Include(script => script.Includes)
            .Where(script => script.AdventureId == adventureId)
            .ToListAsync();
    }

    public async Task SaveChanges()
    {
        await _databaseContext.SaveChangesAsync();
    }
}