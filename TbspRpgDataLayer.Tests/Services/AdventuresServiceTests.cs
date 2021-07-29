using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
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
            var adventures = await service.GetAllAdventures();
            
            // assert
            Assert.Equal(2, adventures.Count);
            Assert.Equal(testAdventure.Id, adventures.First().Id);
        }

        #endregion
    }
}