using System;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories
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
            var adventures = await adventureRepository.GetAllAdventures(new AdventureFilter());
            
            //assert
            Assert.Equal(2, adventures.Count);
            Assert.Equal("TestOne", adventures[0].Name);
            Assert.Equal("TestTwo", adventures[1].Name);
        }
        
        [Fact]
        public async Task GetAllAdventures_FilterCreatedBy_ReturnAdventures()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestOne",
                CreatedByUserId = Guid.NewGuid()
            };
            var testAdventureTwo = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestTwo",
                CreatedByUserId = Guid.NewGuid()
            };
            context.Adventures.AddRange(testAdventure, testAdventureTwo);
            await context.SaveChangesAsync();
            var adventureRepository = new AdventuresRepository(context);
            
            //act
            var adventures = await adventureRepository.GetAllAdventures(
                new AdventureFilter()
                {
                    CreatedBy = testAdventure.CreatedByUserId
                });
            
            //assert
            Assert.Single(adventures);
            Assert.Equal("TestOne", adventures[0].Name);
        }
        
        #endregion

        #region GetPublishedAdventures

        [Fact]
        public async Task GetPublishedAdventures_ReturnPublished()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestOne",
                PublishDate = DateTime.UtcNow
            };
            var testAdventureTwo = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "TestTwo",
                PublishDate = DateTime.UtcNow.AddDays(1)
            };
            context.Adventures.AddRange(testAdventure, testAdventureTwo);
            await context.SaveChangesAsync();
            var adventureRepository = new AdventuresRepository(context);
            
            //act
            var adventures = await adventureRepository.GetPublishedAdventures(new AdventureFilter());
            
            //assert
            Assert.Single(adventures);
            Assert.Equal("TestOne", adventures[0].Name);
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

        #region AddAdventure

        [Fact]
        public async void AddAdventure_AdventureAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var newAdventure = new Adventure()
            {
                Name = "test_adventure",
                CreatedByUserId = Guid.NewGuid(),
                InitialSourceKey = Guid.Empty
            };
            var repository = new AdventuresRepository(context);
            
            // act
            await repository.AddAdventure(newAdventure);
            await repository.SaveChanges();
            
            // assert
            Assert.Single(context.Adventures);
            Assert.Equal("test_adventure", context.Adventures.First().Name);
        }

        #endregion

        #region GetAdventureByIdIncludeAssociatedObjects

        [Fact]
        public async void GetAdventureByIdIncludeAssociatedObjects_Valid_ReturnAdventureWithEverything()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}