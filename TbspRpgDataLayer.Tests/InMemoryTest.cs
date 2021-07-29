using Microsoft.EntityFrameworkCore;
using TbspRpgDataLayer;

namespace TbspRpgDataLayer.Tests
{
    public class InMemoryTest
    {
        protected readonly DbContextOptions<DatabaseContext> DbContextOptions;

        protected InMemoryTest(string dbName)
        {
            DbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            Setup();
        }
        
        private void Setup()
        {
            var databaseContext = new DatabaseContext(DbContextOptions);
            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();
            databaseContext.SaveChanges();
        }
    }
}