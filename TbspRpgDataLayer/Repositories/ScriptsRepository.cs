using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TbspRpgDataLayer.Entities;

namespace TbspRpgDataLayer.Repositories;

public interface IScriptsRepository: IBaseRepository
{
    Task<Script> GetScriptById(Guid scriptId);
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

    public async Task SaveChanges()
    {
        await _databaseContext.SaveChangesAsync();
    }
}