using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
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
                Name = "test location",
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid()
                }
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
        
        #region RemoveLocation

        [Fact]
        public async void RemoveLocation_LocationRemoved()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location"
            };
            var testLocationTwo = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location Two"
            };
            context.Locations.Add(testLocation);
            context.Locations.Add(testLocationTwo);
            await context.SaveChangesAsync();
            var repository = new LocationsRepository(context);
            
            // act
            repository.RemoveLocation(testLocation);
            await repository.SaveChanges();
            
            // assert
            Assert.Single(context.Locations);
        }
        
        #endregion

        #region RemoveLocations

        [Fact]
        public async void RemoveLocations_LocationsRemoved()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location"
            };
            var testLocationTwo = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location Two"
            };
            context.Locations.Add(testLocation);
            context.Locations.Add(testLocationTwo);
            await context.SaveChangesAsync();
            var repository = new LocationsRepository(context);
            
            // act
            repository.RemoveLocations(new List<Location>() { testLocation, testLocationTwo});
            await repository.SaveChanges();
            
            // assert
            Assert.Empty(context.Locations);
        }

        #endregion

        #region GetLocationByIdWithRoutes

        [Fact]
        public async void GetLocationByIdWithRoutes_ValidId_LocationReturnedWithRoutes()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid()
                },
                Routes = new List<Route>()
                {
                    new Route()
                    {
                        Id = Guid.NewGuid(),
                        Name = "route one"
                    },
                    new Route()
                    {
                        Id = Guid.NewGuid(),
                        Name = "route two"
                    }
                }
            };
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
            var repository = new LocationsRepository(context);
            
            // act
            var location = await repository.GetLocationByIdWithRoutes(testLocation.Id);
            
            // assert
            Assert.NotNull(location);
            Assert.Equal(testLocation.Id, location.Id);
            Assert.NotNull(location.Routes);
            Assert.Equal(2, location.Routes.Count);
        }

        #endregion

        #region GetLocationsWithScript

        [Fact]
        public async void GetLocationsWithScript_HasLocations_ReturnLocations()
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
            var repository = new LocationsRepository(context);
            
            // act
            var locations = await repository.GetLocationsWithScript(testLocation.EnterScript.Id);
            
            // assert
            Assert.Equal(2, locations.Count);
            Assert.Null(locations.FirstOrDefault(loc => loc.Id == testLocationThree.Id));
        }

        [Fact]
        public async void GetLocationsWithScript_NoLocations_ReturnEmpty()
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
            var repository = new LocationsRepository(context);
            
            // act
            var locations = await repository.GetLocationsWithScript(Guid.NewGuid());
            
            // assert
            Assert.Empty(locations);
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
            var repository = new LocationsRepository(context);
            
            // act
            var locations = await repository.GetAdventureLocationsWithSource(
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
            var repository = new LocationsRepository(context);
            
            // act
            var locations = await repository.GetAdventureLocationsWithSource(
                testAdventure.Id, testLocation.SourceKey);

            // assert
            Assert.Single(locations);
        }

        #endregion
    }
}