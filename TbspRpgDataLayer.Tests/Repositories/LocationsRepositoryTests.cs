using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories
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

        #region GetLocationsForAdventure

        [Fact]
        public async void GetLocationsForAdventure_ReturnsLocations()
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
            Guid testAdventureIdTwo = Guid.NewGuid();
            var testAdventureTwo = new Adventure
            {
                Id = testAdventureIdTwo,
                Locations = new List<Location>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Initial = true,
                        AdventureId = testAdventureIdTwo
                    }
                },
                Name = "TestTwo"
            };
            context.Adventures.Add(testAdventure);
            context.Adventures.Add(testAdventureTwo);
            await context.SaveChangesAsync();
            var locationRepository = new LocationsRepository(context);
            
            // act
            var locations = await locationRepository.GetLocationsForAdventure(testAdventure.Id);
            
            // assert
            Assert.Equal(2, locations.Count);
        }

        #endregion

        #region GetLocationById

        [Fact]
        public async void GetLocationById_NotExist_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location"
            };
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
            var repository = new LocationsRepository(context);
            
            // act
            var location = await repository.GetLocationById(Guid.NewGuid());
            
            // assert
            Assert.Null(location);
        }

        [Fact]
        public async void GetLocationById_Exists_ReturnLocation()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location"
            };
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
            var repository = new LocationsRepository(context);
            
            // act
            var location = await repository.GetLocationById(testLocation.Id);
            
            // assert
            Assert.NotNull(location);
            Assert.Equal(testLocation.Id, location.Id);
        }

        #endregion

        #region SaveChanges

        [Fact]
        public async void SaveChanges_ChangesAreSaved()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location"
            };
            context.Locations.Add(testLocation);
            var repository = new LocationsRepository(context);
            
            // act
            await repository.SaveChanges();
            
            // assert
            Assert.Single(context.Locations);
        }

        #endregion

        #region AddLocation

        [Fact]
        public async void AddLocation_LocationAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location"
            };
            var repository = new LocationsRepository(context);
            
            // act
            await repository.AddLocation(testLocation);
            await repository.SaveChanges();
            
            // assert
            Assert.Single(context.Locations);
        }

        #endregion
    }
}