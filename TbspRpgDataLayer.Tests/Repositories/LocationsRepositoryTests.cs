using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using TbspRpgDataLayer;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgApi.Tests.Repositories
{
    public class LocationsRepositoryTests : InMemoryTest
    {
        public LocationsRepositoryTests() : base("LocationsRepositoryTests")
        {
        }

        #region GetInitialLocation

        [Fact]
        public async void GetInitialLocation_Valid_ReturnLocation()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventureId = Guid.NewGuid();
            var testAdventure = new Adventure
            {
                Id = testAdventureId,
                Locations = new List<Location>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Initial = true,
                        AdventureId = testAdventureId
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Initial = false,
                        AdventureId = testAdventureId
                    }
                },
                Name = "TestOne"
            };
            context.Adventures.Add(testAdventure);
            await context.SaveChangesAsync();
            var locationRepository = new LocationsRepository(context);
            
            //act
            var location = await locationRepository.GetInitialForAdventure(testAdventureId);

            //assert
            Assert.Equal(testAdventureId, location.AdventureId);
            Assert.True(location.Initial);
        }
        
        [Fact]
        public async void GetInitialLocation_Invalid_ReturnNothing()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventureId = Guid.NewGuid();
            var testAdventure = new Adventure
            {
                Id = testAdventureId,
                Locations = new List<Location>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Initial = true,
                        AdventureId = testAdventureId
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Initial = false,
                        AdventureId = testAdventureId
                    }
                },
                Name = "TestOne"
            };
            context.Adventures.Add(testAdventure);
            await context.SaveChangesAsync();
            var locationRepository = new LocationsRepository(context);
            
            //act
            var location = await locationRepository.GetInitialForAdventure(Guid.NewGuid());
        
            //assert
            Assert.Null(location);
        }
        
        [Fact]
        public async void GetInitialLocation_NoInitial_ReturnNothing()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventureId = Guid.NewGuid();
            var testAdventure = new Adventure
            {
                Id = testAdventureId,
                Locations = new List<Location>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Initial = false,
                        AdventureId = testAdventureId
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Initial = false,
                        AdventureId = testAdventureId
                    }
                },
                Name = "TestOne"
            };
            context.Adventures.Add(testAdventure);
            await context.SaveChangesAsync();
            var locationRepository = new LocationsRepository(context);
            
            //act
            var location = await locationRepository.GetInitialForAdventure(testAdventureId);
        
            //assert
            Assert.Null(location);
        }
        
        [Fact]
        public async void GetInitialLocation_NoLocations_ReturnNothing()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventureId = Guid.NewGuid();
            var testAdventure = new Adventure
            {
                Id = testAdventureId,
                Locations = new List<Location>(),
                Name = "TestOne"
            };
            context.Adventures.Add(testAdventure);
            await context.SaveChangesAsync();
            var locationRepository = new LocationsRepository(context);
            
            //act
            var location = await locationRepository.GetInitialForAdventure(testAdventureId);
        
            //assert
            Assert.Null(location);
        }

        #endregion
    }
}