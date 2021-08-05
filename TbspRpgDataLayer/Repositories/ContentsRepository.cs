using TbspRpgApi.Entities;

namespace TbspRpgDataLayer.Repositories
{
    public interface IContentsRepository : IBaseRepository
    {
        void AddContent(Content content);
    }
    
    public class ContentsRepository : IContentsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public ContentsRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async void AddContent(Content content)
        {
            await _databaseContext.AddAsync(content);
        }
        
        public async void SaveChanges()
        {
            await _databaseContext.SaveChangesAsync();
        }
    }
}