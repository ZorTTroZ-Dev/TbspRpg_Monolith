using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;
using Xunit;

namespace TbspRpgDataLayer.Tests.Services
{
    public class AdventuresServiceTests : InMemoryTest
    {
        public AdventuresServiceTests() : base("AdventuresServiceTests") {}

        private static IAdventuresService CreateService(DatabaseContext context)
        {
            return new AdventuresService(
                new AdventuresRepository(context),
                NullLogger<AdventuresService>.Instance);
        }
        
        #region GetAllAdventures

        [Fact]
        public async void GetAllAdventures_AllAdventuresReturned()
        {
            //  arrange
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
            var service = CreateService(context);
            
            // act
            var adventures = await service.GetAllAdventures(new AdventureFilter());
            
            // assert
            Assert.Equal(2, adventures.Count);
            Assert.Equal(testAdventure.Id, adventures.First().Id);
        }
        
        [Fact]
        public async void GetAllAdventures_FilterCreatedBy_ReturnAdventures()
        {
            //  arrange
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
            var service = CreateService(context);
            
            // act
            var adventures = await service.GetAllAdventures(new AdventureFilter()
            {
                CreatedBy = testAdventure.CreatedByUserId
            });
            
            // assert
            Assert.Single(adventures);
            Assert.Equal(testAdventure.Id, adventures.First().Id);
        }
        
        [Fact]
        public async void GetAllAdventures_FilterCreatedByNone_ReturnEmpty()
        {
            //  arrange
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
            var service = CreateService(context);
            
            // act
            var adventures = await service.GetAllAdventures(new AdventureFilter()
            {
                CreatedBy = Guid.NewGuid()
            });
            
            // assert
            Assert.Empty(adventures);
        }

        #endregion

        #region GetAdventureByName

        [Fact]
        public async void GetAdventureByName_Exact_ReturnAdventure()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            };
            context.Adventures.Add(testAdventure);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var adventure = await service.GetAdventureByName("test");
            
            // assert
            Assert.NotNull(adventure);
            Assert.Equal(testAdventure.Id, adventure.Id);
        }

        [Fact]
        public async void GetAdventureByName_CaseInsensitive_ReturnAdventure()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            };
            context.Adventures.Add(testAdventure);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var adventure = await service.GetAdventureByName("tEsT");
            
            // assert
            Assert.NotNull(adventure);
            Assert.Equal(testAdventure.Id, adventure.Id);
        }

        [Fact]
        public async void GetAdventureByName_Invalid_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            };
            context.Adventures.Add(testAdventure);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var adventure = await service.GetAdventureByName("testy");
            
            // assert
            Assert.Null(adventure);
        }

        #endregion

        #region GetAdventureById

        [Fact]
        public async void GetAdventureById_Exists_ReturnAdventure()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            };
            context.Adventures.Add(testAdventure);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var adventure = await service.GetAdventureById(testAdventure.Id);
            
            // assert
            Assert.NotNull(adventure);
            Assert.Equal(testAdventure.Id, adventure.Id);
        }

        [Fact]
        public async void GetAdventureById_Invalid_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            };
            context.Adventures.Add(testAdventure);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var adventure = await service.GetAdventureById(Guid.NewGuid());
            
            // assert
            Assert.Null(adventure);
        }

        #endregion
    }
}