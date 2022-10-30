using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class RouteProcessorTests: ProcessorTest
    {
        #region UpdateRoute

        [Fact]
        public async void UpdateRoute_InvalidRouteLocationId_ThrowsException()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid()
            };
            var testLocations = new List<Location>() {testLocation};
            var testRoutes = new List<Route>() {testRoute};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations);
            
            // act
            Task Act() => processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                newDestinationLocationName = null,
                route = new Route()
                {
                    Id = testRoute.Id,
                    LocationId = testRoute.LocationId
                },
                source = null,
                successSource = null
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void UpdateRoute_EmptyRouteId_ReturnNewRoute()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = testLocation.AdventureId
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid()
            };
            var testLocations = new List<Location>() {testLocation, testDestinationLocation};
            var testRoutes = new List<Route>() {testRoute};
            var testSources = new List<En>();
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations,
                testSources);
            
            // act
            await processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                newDestinationLocationName = null,
                route = new Route()
                {
                    Id = Guid.Empty,
                    LocationId = testLocation.Id,
                    Name = "new route",
                    SourceKey = Guid.Empty,
                    RouteTakenSourceKey = Guid.Empty,
                    DestinationLocationId = testDestinationLocation.Id
                },
                source = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source text"
                },
                successSource = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source success text"
                }
            });
            
            // assert
            Assert.Equal(2, testRoutes.Count);
            Assert.NotNull(testRoutes.FirstOrDefault(route => route.Name == "new route"));
        }

        [Fact]
        public async void UpdateRoute_InvalidRouteId_ThrowsException()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocation.Id
            };
            var testLocations = new List<Location>() {testLocation};
            var testRoutes = new List<Route>() {testRoute};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations);
            
            // act
            Task Act() => processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                newDestinationLocationName = null,
                route = new Route()
                {
                    Id = Guid.NewGuid(),
                    LocationId = testRoute.LocationId
                },
                source = null,
                successSource = null
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void UpdateRoute_ValidRoute_UpdateExistingRoute()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = testLocation.AdventureId
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route",
                LocationId = testLocation.Id
            };
            var testLocations = new List<Location>() {testLocation, testDestinationLocation};
            var testRoutes = new List<Route>() {testRoute};
            var testSources = new List<En>();
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations,
                testSources);
            
            // act
            await processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                newDestinationLocationName = null,
                route = new Route()
                {
                    Id = testRoute.Id,
                    LocationId = testLocation.Id,
                    Name = "updated route",
                    SourceKey = Guid.Empty,
                    RouteTakenSourceKey = Guid.Empty,
                    DestinationLocationId = testDestinationLocation.Id
                },
                source = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source text"
                },
                successSource = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source success text"
                }
            });
            
            // assert
            Assert.Single(testRoutes);
            Assert.NotNull(testRoutes.FirstOrDefault(route => route.Id == testRoute.Id));
            Assert.Equal("updated route", testRoute.Name);
            Assert.NotEqual(Guid.Empty, testRoute.SourceKey);
            Assert.NotEqual(Guid.Empty, testRoute.RouteTakenSourceKey);
        }

        [Fact]
        public async void UpdateRoute_UpdateExistingSource_SourceUpdated()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = testLocation.AdventureId
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route",
                LocationId = testLocation.Id
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testLocation.AdventureId,
                Name = "test source",
                Text = "source text"
            };
            var testSuccessSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testLocation.AdventureId,
                Name = "test success source",
                Text = "success source text"
            };
            var testLocations = new List<Location>() {testLocation, testDestinationLocation};
            var testRoutes = new List<Route>() {testRoute};
            var testSources = new List<En>() {testSource, testSuccessSource};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations,
                testSources);
            
            // act
            await processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                newDestinationLocationName = null,
                route = new Route()
                {
                    Id = testRoute.Id,
                    LocationId = testLocation.Id,
                    Name = "updated route",
                    SourceKey = testSource.Key,
                    RouteTakenSourceKey = testSuccessSource.Key,
                    DestinationLocationId = testDestinationLocation.Id
                },
                source = new En()
                {
                    Key = testSource.Key,
                    AdventureId = testSource.AdventureId,
                    Text = "updated source text"
                },
                successSource = new En()
                {
                    Key = testSuccessSource.Key,
                    AdventureId = testSuccessSource.AdventureId,
                    Text = "updated success source text"
                }
            });
            
            // assert
            Assert.Single(testRoutes);
            Assert.Equal(2, testSources.Count);
            Assert.NotEqual(Guid.Empty, testRoute.SourceKey);
            Assert.NotEqual(Guid.Empty, testRoute.RouteTakenSourceKey);
            Assert.NotNull(testSources.FirstOrDefault(source => source.Key == testRoute.SourceKey));
            Assert.NotNull(testSources.FirstOrDefault(source => source.Key == testRoute.RouteTakenSourceKey));
            var source = testSources.First(source => source.Key == testRoute.SourceKey);
            Assert.Equal("updated source text", source.Text);
            var successSource = testSources.First(src => src.Key == testRoute.RouteTakenSourceKey);
            Assert.Equal("updated success source text", successSource.Text);
        }

        [Fact]
        public async void UpdateRoute_CreateSource_SourceCreated()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = testLocation.AdventureId
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route",
                LocationId = testLocation.Id
            };
            var testLocations = new List<Location>() {testLocation, testDestinationLocation};
            var testRoutes = new List<Route>() {testRoute};
            var testSources = new List<En>();
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations,
                testSources);
            
            // act
            await processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                newDestinationLocationName = null,
                route = new Route()
                {
                    Id = testRoute.Id,
                    LocationId = testLocation.Id,
                    Name = "updated route",
                    SourceKey = Guid.Empty,
                    RouteTakenSourceKey = Guid.Empty,
                    DestinationLocationId = testDestinationLocation.Id
                },
                source = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source text"
                },
                successSource = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source success text"
                }
            });
            
            // assert
            Assert.Single(testRoutes);
            Assert.Equal(2, testSources.Count);
            Assert.NotEqual(Guid.Empty, testRoute.SourceKey);
            Assert.NotEqual(Guid.Empty, testRoute.RouteTakenSourceKey);
            Assert.NotNull(testSources.FirstOrDefault(source => source.Key == testRoute.SourceKey));
            Assert.NotNull(testSources.FirstOrDefault(source => source.Key == testRoute.RouteTakenSourceKey));
            var source = testSources.First(source => source.Key == testRoute.SourceKey);
            Assert.Equal("source text", source.Text);
        }

        [Fact]
        public async void UpdateRoute_NewDestinationName_CreateNewDestinationLocation()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = testLocation.AdventureId
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route",
                LocationId = testLocation.Id
            };
            var testLocations = new List<Location>() {testLocation, testDestinationLocation};
            var testRoutes = new List<Route>() {testRoute};
            var testSources = new List<En>();
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations,
                testSources);
            
            // act
            await processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                newDestinationLocationName = "bananas",
                route = new Route()
                {
                    Id = testRoute.Id,
                    LocationId = testLocation.Id,
                    Name = "updated route",
                    SourceKey = Guid.Empty,
                    RouteTakenSourceKey = Guid.Empty,
                    DestinationLocationId = testDestinationLocation.Id
                },
                source = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source text"
                },
                successSource = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source success text"
                }
            });
            
            // assert
            Assert.Single(testRoutes);
            Assert.NotEqual(testDestinationLocation.Id, testRoutes[0].DestinationLocationId);
            Assert.NotNull(testLocations.FirstOrDefault(location => location.Name == "bananas"));
            Assert.Equal(3, testLocations.Count);
        }

        [Fact]
        public async void UpdateRoute_ChangeDestinationLocation_LocationUpdated()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = testLocation.AdventureId
            };
            var newTestDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = testLocation.AdventureId
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route",
                LocationId = testLocation.Id,
                DestinationLocationId = testDestinationLocation.Id
            };
            var testLocations = new List<Location>() {testLocation,
                testDestinationLocation, newTestDestinationLocation};
            var testRoutes = new List<Route>() {testRoute};
            var testSources = new List<En>();
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations,
                testSources);
            
            // act
            await processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                route = new Route()
                {
                    Id = testRoute.Id,
                    LocationId = testLocation.Id,
                    Name = "updated route",
                    SourceKey = Guid.Empty,
                    RouteTakenSourceKey = Guid.Empty,
                    DestinationLocationId = newTestDestinationLocation.Id
                },
                source = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source text"
                },
                successSource = new En()
                {
                    Key = Guid.Empty,
                    AdventureId = Guid.Empty,
                    Text = "source success text"
                }
            });
            
            // assert
            Assert.Single(testRoutes);
            Assert.Equal(newTestDestinationLocation.Id, testRoutes[0].DestinationLocationId);
            Assert.Equal(3, testLocations.Count);
        }
        
        [Fact]
        public async void UpdateRoute_InvalidDestinationLocationId_ThrowsException()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocation.Id
            };
            var testLocations = new List<Location>() {testLocation};
            var testRoutes = new List<Route>() {testRoute};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations);
            
            // act
            Task Act() => processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                newDestinationLocationName = null,
                route = new Route()
                {
                    Id = testRoute.Id,
                    LocationId = testRoute.LocationId,
                    DestinationLocationId = Guid.NewGuid()
                },
                source = null,
                successSource = null
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void UpdateRoute_EmptyDestinationLocationId_ThrowsException()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocation.Id
            };
            var testLocations = new List<Location>() {testLocation};
            var testRoutes = new List<Route>() {testRoute};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations);
            
            // act
            Task Act() => processor.UpdateRoute(new RouteUpdateModel()
            {
                language = Languages.ENGLISH,
                newDestinationLocationName = null,
                route = new Route()
                {
                    Id = testRoute.Id,
                    LocationId = testRoute.LocationId,
                    DestinationLocationId = Guid.Empty
                },
                source = null,
                successSource = null
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion

        #region RemoveRoutes

        [Fact]
        public async void RemoveRoutes_RoutesExist_RoutesRemoved()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route",
                LocationId = Guid.NewGuid()
            };
            var testRouteTwo = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route two",
                LocationId = testRoute.LocationId
            };
            var testRoutes = new List<Route>() {testRoute, testRouteTwo};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes);
            
            // act
            await processor.RemoveRoutes(new RoutesRemoveModel() {
                CurrentRouteIds = new List<Guid>() { testRouteTwo.Id },
                LocationId = testRoute.LocationId
            });
            
            // assert
            Assert.Single(testRoutes);
            Assert.Equal("existing route two", testRoutes[0].Name);
        }

        [Fact]
        public async void RemoveRoutes_NoExtraRoutes_NoRoutesRemoved()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route",
                LocationId = Guid.NewGuid()
            };
            var testRouteTwo = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route two",
                LocationId = testRoute.LocationId
            };
            var testRoutes = new List<Route>() {testRoute, testRouteTwo};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes);
            
            // act
            await processor.RemoveRoutes(new RoutesRemoveModel() {
                CurrentRouteIds = new List<Guid>() { testRoute.Id, testRouteTwo.Id },
                LocationId = testRoute.LocationId
            });
            
            // assert
            Assert.Equal(2, testRoutes.Count);
        }
        
        [Fact]
        public async void RemoveRoutes_NoRoutes_AllRoutesRemoved()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route",
                LocationId = Guid.NewGuid()
            };
            var testRouteTwo = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "existing route two",
                LocationId = testRoute.LocationId
            };
            var testRoutes = new List<Route>() {testRoute, testRouteTwo};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes);
            
            // act
            await processor.RemoveRoutes(new RoutesRemoveModel() {
                CurrentRouteIds = new List<Guid>(),
                LocationId = testRoute.LocationId
            });
            
            // assert
            Assert.Empty(testRoutes);
        }

        #endregion

        #region RemoveRoute

        [Fact]
        public async void RemoveRoute_InvalidRouteId_ThrowException()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid()
            };
            var testLocations = new List<Location>() {testLocation};
            var testRoutes = new List<Route>() {testRoute};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations);
            
            // act
            Task Act() => processor.RemoveRoute(new RouteRemoveModel()
            {
                RouteId = Guid.NewGuid()
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void RemoveRoute_ValidRouteId_RouteRemoved()
        {
            // arrange
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid()
            };
            var testLocations = new List<Location>() {testLocation};
            var testRoutes = new List<Route>() {testRoute};
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                testRoutes,
                testLocations);
            
            // act
            await processor.RemoveRoute(new RouteRemoveModel()
            {
                RouteId = testRoute.Id
            });
            
            // assert
            Assert.Empty(testRoutes);
        }

        #endregion
    }
}