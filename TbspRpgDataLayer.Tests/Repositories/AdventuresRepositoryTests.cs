using System;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgDataLayer;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgApi.Tests.Repositories
{
    public class AdventuresRepositoryTests : InMemoryTest
    {
        public AdventuresRepositoryTests() : base("AdventuresRepositoryTests")
        {
        }
        
        #region GetAllAdventures
        
        [Fact]
        public async Task GetAllAdventures_ReturnAll()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestOne"
            };
            var testAdventureTwo = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestTwo"
            };
            context.Adventures.AddRange(testAdventure, testAdventureTwo);
            await context.SaveChangesAsync();
            var adventureRepository = new AdventuresRepository(context);
            
            //act
            var adventures = await adventureRepository.GetAllAdventures();
            
            //assert
            Assert.Equal(2, adventures.Count);
            Assert.Equal("TestOne", adventures[0].Name);
            Assert.Equal("TestTwo", adventures[1].Name);
        }
        
        #endregion
        
        #region GetAdventureByName
        
        [Fact]
        public async Task GetAdventureByName_ReturnAdventure()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestOne"
            };
            context.Adventures.AddRange(testAdventure);
            await context.SaveChangesAsync();
            var adventureRepository = new AdventuresRepository(context);
            
            //act
            var adventure = await adventureRepository.GetAdventureByName("TestOne");
            
            //assert
            Assert.Equal("TestOne", adventure.Name);
        }
        
        [Fact]
        public async Task GetAdventureByName_CaseInsensitive_ReturnAdventure()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestOne"
            };
            var testAdventureTwo = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestTwo"
            };
            context.Adventures.AddRange(testAdventure, testAdventureTwo);
            await context.SaveChangesAsync();
            var adventureRepository = new AdventuresRepository(context);
            
            //act
            var adventure = await adventureRepository.GetAdventureByName("testTWO");
            
            //assert
            Assert.Equal("TestTwo", adventure.Name);
        }
        
        [Fact]
        public async Task GetAdventureByName_InvalidName_ReturnNothing()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestOne"
            };
            context.Adventures.AddRange(testAdventure);
            await context.SaveChangesAsync();
            var adventureRepository = new AdventuresRepository(context);
            
            //act
            var adventure = await adventureRepository.GetAdventureByName("invalid");
            
            //assert
            Assert.Null(adventure);
        }
        
        #endregion
        
        #region GetAdventureById
        
        [Fact]
        public async Task GetAdventureById_ReturnAdventure()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestOne"
            };
            context.Adventures.AddRange(testAdventure);
            await context.SaveChangesAsync();
            var adventureRepository = new AdventuresRepository(context);
            
            //act
            var adventure = await adventureRepository.GetAdventureById(testAdventure.Id);
            
            //assert
            Assert.Equal("TestOne", adventure.Name);
        }
        
        [Fact]
        public async Task GetAdventureById_Invalid_ReturnNothing()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestOne"
            };
            context.Adventures.AddRange(testAdventure);
            await context.SaveChangesAsync();
            var adventureRepository = new AdventuresRepository(context);
            
            //act
            var adventure = await adventureRepository.GetAdventureById(Guid.NewGuid());
            
            //assert
            Assert.Null(adventure);
        }
        
        #endregion
    }
}