using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;
using Xunit;

namespace TbspRpgDataLayer.Tests.Services
{
    public class LocationsServiceTests : InMemoryTest
    {
        public LocationsServiceTests() : base("LocationsServiceTests")
        {
        }

        private static ILocationsService CreateService(DatabaseContext context)
        {
            return new LocationsService(new LocationsRepository(context),
                NullLogger<LocationsService>.Instance);
        }

        #region GetInitialLocationForAdventure

        [Fact]
        public async void GetInitialLocationForAdventure_Valid_ReturnLocation()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Initial = true
            };
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var location = await service.GetInitialLocationForAdventure(testLocation.AdventureId);
            
            // assert
            Assert.NotNull(location);
            Assert.Equal(testLocation.Id, location.Id);
        }
        
        [Fact]
        public async void GetInitialLocationForAdventure_InvalidAdventure_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Initial = true
            };
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var location = await service.GetInitialLocationForAdventure(Guid.NewGuid());
            
            // assert
            Assert.Null(location);
        }
        
        [Fact]
        public async void GetInitialLocationForAdventure_NoInitial_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Initial = false
            };
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var location = await service.GetInitialLocationForAdventure(testLocation.AdventureId);
            
            // assert
            Assert.Null(location);
        }

        #endregion

        #region GetLocationsForAdventure

        [Fact]
        public async void GetLocationsForAdventure_ReturnsLocations()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Initial = false
            };
            var testLocationTwo = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Initial = false
            };
            context.Locations.Add(testLocation);
            context.Locations.Add(testLocationTwo);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var locations = await service.GetLocationsForAdventure(testLocation.AdventureId);
            
            // assert
            Assert.Single(locations);
            Assert.Equal(testLocation.Id, locations[0].Id);
        }

        #endregion

        #region GetLocationById

        [Fact]
        public async void GetLocationById_Exists_ReturnLocation()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var location = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid()
                }
            };
            context.Locations.Add(location);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var dblocation = await service.GetLocationById(location.Id);
            
            // assert
            Assert.NotNull(dblocation);
            Assert.Equal(location.Id, dblocation.Id);
        }

        #endregion

        #region SaveChanges

        [Fact]
        public async void SaveChanges_SavesChanges()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var location = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location"
            };
            context.Locations.Add(location);
            var service = CreateService(context);
            
            // act
            await service.SaveChanges();
            
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
            var service = CreateService(context);
            
            // act
            await service.AddLocation(testLocation);
            await service.SaveChanges();
            
            // assert
            Assert.Single(context.Locations);
        }
        
        #endregion

        #region RemoveScriptFromLocations

        [Fact]
        public async void RemoveScriptFromLocations_ScriptRemoved()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                EnterScript = new Script()
                {
                    Id = Guid.NewGuid(),
                    Name = "test script"
                }
            };
            var testLocationTwo = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location two",
                EnterScript = testLocation.EnterScript
            };
            var testLocationThree = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location three"
            };
            await context.Locations.AddRangeAsync(testLocation, testLocationTwo, testLocationThree);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            await service.RemoveScriptFromLocations(testLocation.EnterScript.Id);
            await service.SaveChanges();
            
            // assert
            Assert.Null(context.Locations.First(loc => loc.Id == testLocation.Id).EnterScript);
            Assert.Null(context.Locations.First(loc => loc.Id == testLocationTwo.Id).ExitScript);
        }

        #endregion
        
        #region GetAdventureLocationsWithSource

        [Fact]
        public async void GetAdventureLocationsWithSource_NoLocations_ReturnEmpty()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid()
            };
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Adventure = testAdventure,
                SourceKey = Guid.NewGuid()
            };
            var testLocationTwo = new Location()
            {
                Id = Guid.NewGuid(),
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid()
                },
                SourceKey = Guid.NewGuid()
            };
            await context.Adventures.AddRangeAsync(testAdventure);
            await context.Locations.AddRangeAsync(testLocation, testLocationTwo);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var locations = await service.GetAdventureLocationsWithSource(
                Guid.NewGuid(), testLocation.SourceKey);

            // assert
            Assert.Empty(locations);
        }

        [Fact]
        public async void GetAdventureLocationsWithSource_Exists_ReturnLocations()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid()
            };
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Adventure = testAdventure,
                SourceKey = Guid.NewGuid()
            };
            var testLocationTwo = new Location()
            {
                Id = Guid.NewGuid(),
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid()
                },
                SourceKey = Guid.NewGuid()
            };
            await context.Adventures.AddRangeAsync(testAdventure);
            await context.Locations.AddRangeAsync(testLocation, testLocationTwo);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var locations = await service.GetAdventureLocationsWithSource(
                testAdventure.Id, testLocation.SourceKey);

            // assert
            Assert.Single(locations);
        }

        #endregion

        #region DoesAdventureLocationUseSource

        [Fact]
        public async void DoesAdventLocationUseSource_DoesUseSource_ReturnTrue()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid()
            };
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Adventure = testAdventure,
                SourceKey = Guid.NewGuid()
            };
            var testLocationTwo = new Location()
            {
                Id = Guid.NewGuid(),
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid()
                },
                SourceKey = Guid.NewGuid()
            };
            await context.Adventures.AddRangeAsync(testAdventure);
            await context.Locations.AddRangeAsync(testLocation, testLocationTwo);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var usesSource = await service.DoesAdventureLocationUseSource(
                testAdventure.Id, testLocation.SourceKey);
            
            // assert
            Assert.True(usesSource);
        }
        
        [Fact]
        public async void DoesAdventLocationUseSource_DoesntUseSource_ReturnFalse()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid()
            };
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Adventure = testAdventure,
                SourceKey = Guid.NewGuid()
            };
            var testLocationTwo = new Location()
            {
                Id = Guid.NewGuid(),
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid()
                },
                SourceKey = Guid.NewGuid()
            };
            await context.Adventures.AddRangeAsync(testAdventure);
            await context.Locations.AddRangeAsync(testLocation, testLocationTwo);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var usesSource = await service.DoesAdventureLocationUseSource(
                Guid.NewGuid(), testLocation.SourceKey);
            
            // assert
            Assert.False(usesSource);
        }

        #endregion
    }
}