using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class LocationProcessorTests : ProcessorTest
    {
        #region UpdateLocation

        [Fact]
        public async void UpdateLocation_BadLocationId_ThrowException()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Initial = true,
                SourceKey = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "test source"
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>() {testSource};
            var processor = CreateLocationProcessor(locations, sources);
            
            // act
            Task Act() => processor.UpdateLocation(new Location()
            {
                Id = Guid.NewGuid(),
                Name = "updated location name",
                Initial = false,
                SourceKey = testLocation.SourceKey
            }, new En()
            {
                Id = testSource.Id,
                Key = testSource.Key,
                Name = "test location",
                Text = "updated source"
            }, Languages.ENGLISH);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void UpdateLocation_NewSource_NewSourceCreated()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Initial = true,
                SourceKey = Guid.Empty
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>();
            var processor = CreateLocationProcessor(locations, sources);
            
            // act
            await processor.UpdateLocation(new Location()
            {
                Id = testLocation.Id,
                Name = "updated location name",
                Initial = true
            }, new En()
            {
                Key = Guid.Empty,
                Text = "updated source"
            }, Languages.ENGLISH);

            // assert
            Assert.Single(sources);
            Assert.Single(locations);
            Assert.Equal("updated location name", sources[0].Name);
            Assert.Equal("updated source", sources[0].Text);
            Assert.Equal(sources[0].Key, locations[0].SourceKey);
        }

        [Fact]
        public async void UpdateLocation_BadSourceId_ThrowException()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Initial = true,
                SourceKey = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "test source"
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>() {testSource};
            var processor = CreateLocationProcessor(locations, sources);
            
            // act
            Task Act() => processor.UpdateLocation(new Location()
            {
                Id = testLocation.Id,
                Name = "updated location name",
                Initial = false,
                SourceKey = testLocation.SourceKey
            }, new En()
            {
                Id = testSource.Id,
                Key = Guid.NewGuid(),
                Name = "test location",
                Text = "updated source"
            }, Languages.ENGLISH);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void UpdateLocation_UpdateLocationAndSource_LocationSourceUpdated()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Name = "test location",
                Initial = true,
                SourceKey = Guid.NewGuid()
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "test source"
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>() {testSource};
            var processor = CreateLocationProcessor(locations, sources);
            
            // act
            await processor.UpdateLocation(new Location()
            {
                Id = testLocation.Id,
                Name = "updated location name",
                Initial = false,
                SourceKey = testLocation.SourceKey
            }, new En()
            {
                Id = testSource.Id,
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "updated source"
            }, Languages.ENGLISH);
            
            // assert
            Assert.Single(sources);
            Assert.Single(locations);
            Assert.False(locations[0].Initial);
            Assert.Equal("updated location name", locations[0].Name);
            Assert.Equal("updated source", sources[0].Text);
        }

        [Fact]
        public async void UpdateLocation_EmptyLocaitonId_CreateNewLocation()
        {
            // arrange
            var locations = new List<Location>();
            var sources = new List<En>();
            var processor = CreateLocationProcessor(locations, sources);
            
            // act
            await processor.UpdateLocation(new Location()
            {
                Id = Guid.Empty,
                Name = "new location name",
                Initial = false,
                SourceKey = Guid.Empty
            }, new En()
            {
                Key = Guid.Empty,
                Name = "new location name",
                Text = "updated source"
            }, Languages.ENGLISH);
            
            // assert
            Assert.Single(sources);
            Assert.Single(locations);
            Assert.False(locations[0].Initial);
            Assert.Equal("new location name", locations[0].Name);
            Assert.Equal("updated source", sources[0].Text);
        }

        #endregion

        #region RemoveLocation

        [Fact]
        public async void RemoveLocation_Valid_LocationRemoved()
        {
            // arrange
            var locationId = Guid.NewGuid();
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route",
                LocationId = locationId
            };
            var testLocation = new Location()
            {
                Id = locationId,
                Name = "test location",
                Initial = true,
                SourceKey = Guid.NewGuid(),
                Routes = new List<Route>()
                {
                    testRoute
                }
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "test source"
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>() {testSource};
            var routes = new List<Route>() {testRoute};
            var processor = CreateLocationProcessor(locations, sources, routes);
            
            // act
            await processor.RemoveLocation(new LocationRemoveModel()
            {
                LocationId = locationId
            });
            
            // assert
            Assert.Empty(locations);
            Assert.Empty(routes);
        }

        [Fact]
        public async void RemoveLocation_InvalidLocationId_ExceptionThrown()
        {
            // arrange
            var locationId = Guid.NewGuid();
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route",
                LocationId = locationId
            };
            var testLocation = new Location()
            {
                Id = locationId,
                Name = "test location",
                Initial = true,
                SourceKey = Guid.NewGuid(),
                Routes = new List<Route>()
                {
                    testRoute
                }
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "test source"
            };
            var locations = new List<Location>() { testLocation };
            var sources = new List<En>() {testSource};
            var routes = new List<Route>() {testRoute};
            var processor = CreateLocationProcessor(locations, sources, routes);
            
            // act
            Task Act() => processor.RemoveLocation(new LocationRemoveModel()
            {
                LocationId = Guid.NewGuid()
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion

        #region RemoveLocations

        [Fact]
        public async void RemoveLocations_LocationsRemoved()
        {
            // arrange
            var locationId = Guid.NewGuid();
            var locationIdTwo = Guid.NewGuid();
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route",
                LocationId = locationId
            };
            var testRouteTwo = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route two",
                LocationId = locationId
            };
            var testLocation = new Location()
            {
                Id = locationId,
                Name = "test location",
                Initial = true,
                SourceKey = Guid.NewGuid(),
                Routes = new List<Route>()
                {
                    testRoute
                }
            };
            var testLocationTwo = new Location()
            {
                Id = locationIdTwo,
                Name = "test location two",
                Initial = true,
                SourceKey = Guid.NewGuid(),
                Routes = new List<Route>()
                {
                    testRouteTwo
                }
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = testLocation.SourceKey,
                Name = "test location",
                Text = "test source"
            };
            var locations = new List<Location>() { testLocation, testLocationTwo };
            var sources = new List<En>() { testSource };
            var routes = new List<Route>() { testRoute, testRouteTwo };
            var processor = CreateLocationProcessor(locations, sources, routes);
            
            // act
            await processor.RemoveLocations(new List<Location>()
            {
                testLocation, testLocationTwo
            });
            
            // assert
            Assert.Empty(locations);
            Assert.Empty(routes);
        }

        #endregion
    }
}