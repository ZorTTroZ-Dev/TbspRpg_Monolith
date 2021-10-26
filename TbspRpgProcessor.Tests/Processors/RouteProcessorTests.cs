using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
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
            var processor = CreateRouteProcessor(testRoutes, testLocations);
            
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
            var processor = CreateRouteProcessor(testRoutes, testLocations, testSources);
            
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
                    SuccessSourceKey = Guid.Empty,
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
            var processor = CreateRouteProcessor(testRoutes, testLocations);
            
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
            var processor = CreateRouteProcessor(testRoutes, testLocations, testSources);
            
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
                    SuccessSourceKey = Guid.Empty,
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
            Assert.NotEqual(Guid.Empty, testRoute.SuccessSourceKey);
        }

        [Fact]
        public async void UpdateRoute_UpdateExistingSource_SourceUpdated()
        {
            
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
            var processor = CreateRouteProcessor(testRoutes, testLocations, testSources);
            
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
                    SuccessSourceKey = Guid.Empty,
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
            Assert.NotEqual(Guid.Empty, testRoute.SuccessSourceKey);
            Assert.NotNull(testSources.FirstOrDefault(source => source.Key == testRoute.SourceKey));
            Assert.NotNull(testSources.FirstOrDefault(source => source.Key == testRoute.SuccessSourceKey));
            var source = testSources.FirstOrDefault(source => source.Key == testRoute.SourceKey);
            Assert.Equal("source text", source.Text);
        }

        [Fact]
        public async void UpdateRoute_NewDestinationName_CreateNewDestinationLocation()
        {
            
        }

        [Fact]
        public async void UpdateRoute_ChangeDestinationLocation_LocationUpdated()
        {
            
        }

        #endregion

        #region RemoveRoutes

        [Fact]
        public async void RemoveRoutes_RoutesExist_RoutesRemoved()
        {
            
        }

        [Fact]
        public async void RemoveRoutes_RoutesNotExist_NoRoutesRemoved()
        {
            
        }

        #endregion
    }
}