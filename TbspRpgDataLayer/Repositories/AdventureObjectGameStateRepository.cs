using System.Threading.Tasks;

namespace TbspRpgDataLayer.Repositories;

public interface IAdventureObjectGameStateRepository : IBaseRepository
{
    
}

public class AdventureObjectGameStateRepository: IAdventureObjectGameStateRepository
{
    private readonly DatabaseContext _databaseContext;

    public AdventureObjectGameStateRepository(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
    
    public async Task SaveChanges()
    {
        await _databaseContext.SaveChangesAsync();
    }
}