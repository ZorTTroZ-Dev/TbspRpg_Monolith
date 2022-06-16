using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;
using Xunit;

namespace TbspRpgDataLayer.Tests.Services
{
    public class RoutesServiceTests : InMemoryTest
    {
        public RoutesServiceTests() : base("RoutesServiceTests")
        {
        }

        private static IRoutesService CreateService(DatabaseContext context)
        {
            return new RoutesService(
                new RoutesRepository(context),
                NullLogger<RoutesService>.Instance);
        }

        #region GetRoutesForLocation

        [Fact]
        public async void GetRoutesForLocation_Valid_ReturnRoutes()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocationId = Guid.NewGuid();
            var testLocation = new Location()
            {
                Id = testLocationId,
                Name = "test location",
                Routes = new List<Route>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "route1",
                        LocationId = testLocationId
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "route2",
                        LocationId = testLocationId
                    }
                }
            };
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var routes = await service.GetRoutesForLocation(testLocationId);
            
            // assert
            Assert.Equal(2, routes.Count);
            Assert.Equal(testLocationId, routes[0].LocationId);
        }
        
        [Fact]
        public async void GetRoutesForLocation_InValidLocation_NoRoutes()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocationId = Guid.NewGuid();
            var testLocation = new Location()
            {
                Id = testLocationId,
                Name = "test location",
                Routes = new List<Route>()
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "route1",
                        LocationId = testLocationId
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "route2",
                        LocationId = testLocationId
                    }
                }
            };
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var routes = await service.GetRoutesForLocation(Guid.NewGuid());
            
            // assert
            Assert.Empty(routes);
        }

        #endregion

        #region GetRouteById

        [Fact]
        public async void GetRouteById_Valid_ReturnRoute()
        {
            // arrange
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid()
            };
            var testLocation = new Location()
            {
                Id = Guid.NewGuid()
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route",
                DestinationLocationId = testDestinationLocation.Id,
                LocationId = testLocation.Id
            };
            await using var context = new DatabaseContext(DbContextOptions);
            context.Routes.Add(testRoute);
            context.Locations.Add(testDestinationLocation);
            context.Locations.Add(testLocation);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var route = await service.GetRouteById(testRoute.Id);
            
            // assert
            Assert.NotNull(route);
            Assert.Equal(testRoute.Id, route.Id);
        }
        
        [Fact]
        public async void GetRouteById_InValid_ReturnNull()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route"
            };
            await using var context = new DatabaseContext(DbContextOptions);
            context.Routes.Add(testRoute);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var route = await service.GetRouteById(Guid.NewGuid());
            
            // assert
            Assert.Null(route);
        }

        #endregion
        
        #region GetRoutes

        [Fact]
        public async void GetRoutes_FilterByDestinationLocationId_ReturnsRoutes()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testroute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                Name = "test route"
            };
            var testroute2 = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                Name = "test route two"
            };
            context.Routes.AddRange(testroute, testroute2);
            await context.SaveChangesAsync();
            var service = CreateService(context);

            // act
            var routes = await service.GetRoutes(new RouteFilter()
            {
                DestinationLocationId = testroute.DestinationLocationId
            });
            
            // assert
            Assert.Single(routes);
            Assert.Equal("test route", routes[0].Name);
        }
        
        [Fact]
        public async void GetRoutes_FilterByLocationId_ReturnsRoutes()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testroute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                Name = "test route"
            };
            var testroute2 = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                Name = "test route two"
            };
            context.Routes.AddRange(testroute, testroute2);
            await context.SaveChangesAsync();
            var service = CreateService(context);

            // act
            var routes = await service.GetRoutes(new RouteFilter()
            {
                LocationId = testroute.LocationId
            });
            
            // assert
            Assert.Single(routes);
            Assert.Equal("test route", routes[0].Name);
        }
        
        [Fact]
        public async void GetRoutes_NoFilter_ReturnsAll()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testroute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                Name = "test route"
            };
            var testroute2 = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                Name = "test route two"
            };
            context.Routes.AddRange(testroute, testroute2);
            await context.SaveChangesAsync();
            var service = CreateService(context);

            // act
            var routes = await service.GetRoutes(null);
            
            // assert
            Assert.Equal(2, routes.Count);
        }

        #endregion

        #region RemoveRoute

        [Fact]
        public async void RemoveRoute_RouteRemoved()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testroute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                Name = "test route"
            };
            context.Routes.Add(testroute);
            await context.SaveChangesAsync();
            var service = CreateService(context);

            // act
            var routeToRemove = context.Routes.First(route => route.Id == testroute.Id);
            service.RemoveRoute(routeToRemove);
            
            // assert
            await context.SaveChangesAsync();
            Assert.Empty(context.Routes);
        }

        #endregion
        
        #region AddRoute

        [Fact]
        public async void AddRoute_RouteAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var service = CreateService(context);
            
            // act
            await service.AddRoute(new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route",
                LocationId = Guid.NewGuid(),
                SourceKey = Guid.NewGuid()
            });
            await service.SaveChanges();
            
            // assert
            Assert.Single(context.Routes);
            Assert.Equal("test route", context.Routes.First().Name);
        }

        #endregion

        #region RemoveScriptFromRoutes

        [Fact]
        public async void RemoveScriptFromRoutes_ScriptRemoved()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route",
                RouteTakenScript = new Script()
                {
                    Id = Guid.NewGuid(),
                    Name = "test script"
                }
            };
            var testRouteTwo = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route two" 
            };
            await using var context = new DatabaseContext(DbContextOptions);
            await context.Routes.AddRangeAsync(testRoute, testRouteTwo);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            service.RemoveScriptFromRoutes(testRoute.RouteTakenScript.Id);
            await service.SaveChanges();
            
            // assert
            Assert.Null(context.Routes.First(route => route.Id == testRoute.Id).RouteTakenScript);
        }

        #endregion
    }
}