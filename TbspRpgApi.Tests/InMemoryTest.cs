using Microsoft.EntityFrameworkCore;
using TbspRpgApi.Repositories;

namespace TbspRpgApi.Tests
{
    public class InMemoryTest
    {
        protected readonly DbContextOptions<DatabaseContext> DbContextOptions;

        protected InMemoryTest(string dbName)
        {
            DbContextOptions = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
        }
    }
}