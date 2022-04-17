using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.ArgumentModels;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories
{
    public class RoutesRepositoryTests : InMemoryTest
    {
        public RoutesRepositoryTests() : base("RoutesRepositoryTests")
        {
        }
        
        #region GetRoutesForLocation

        [Fact]
        public async void GetRoutesForLocation_ValidLocation_RoutesReturned()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test",
                LocationId = Guid.NewGuid()
            };
            var testRouteTwo = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test_two",
                LocationId = testRoute.LocationId
            };
            context.Routes.AddRange(testRoute, testRouteTwo);
            await context.SaveChangesAsync();
            var repo = new RoutesRepository(context);
            
            //act
            var routes = await repo.GetRoutesForLocation(testRoute.LocationId);
            
            //assert
            Assert.Equal(2, routes.Count);
            Assert.Equal(testRoute.Name, routes.First().Name);
        }

        [Fact]
        public async void GetRoutesForLocation_InvalidLocation_NoRoutes()
        {
            //arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test",
                LocationId = Guid.NewGuid()
            };
            context.Routes.Add(testRoute);
            await context.SaveChangesAsync();
            var repo = new RoutesRepository(context);
            
            //act
            var routes = await repo.GetRoutesForLocation(Guid.NewGuid());
            
            //assert
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
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route",
                DestinationLocationId = testDestinationLocation.Id
            };
            await using var context = new DatabaseContext(DbContextOptions);
            context.Routes.Add(testRoute);
            context.Locations.Add(testDestinationLocation);
            await context.SaveChangesAsync();
            var repository = new RoutesRepository(context);
            
            // act
            var route = await repository.GetRouteById(testRoute.Id);
            
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
            var repository = new RoutesRepository(context);
            
            // act
            var route = await repository.GetRouteById(Guid.NewGuid());
            
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
            var repository = new RoutesRepository(context);
            
            // act
            var routes = await repository.GetRoutes(new RouteFilter()
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
            var repository = new RoutesRepository(context);
            
            // act
            var routes = await repository.GetRoutes(new RouteFilter()
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
            var repository = new RoutesRepository(context);
            
            // act
            var routes = await repository.GetRoutes(null);
            
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
            var testroute2 = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                Name = "test route two"
            };
            context.Routes.AddRange(testroute, testroute2);
            await context.SaveChangesAsync();
            var repository = new RoutesRepository(context);
            
            // act
            var routeToRemove = context.Routes.First(route => route.Id == testroute.Id);
            repository.RemoveRoute(routeToRemove);
            
            // assert
            await context.SaveChangesAsync();
            Assert.Single(context.Routes);
            Assert.Equal("test route two", context.Routes.First().Name);
        }

        #endregion
        
        #region RemoveRoutes

        [Fact]
        public async void RemoveRoutes_RoutesRemoved()
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
            var repository = new RoutesRepository(context);
            
            // act
            repository.RemoveRoutes(new List<Route>() { testroute, testroute2 });
            await context.SaveChangesAsync();
            
            // assert
            Assert.Empty(context.Routes);
        }

        #endregion

        #region AddRoute

        [Fact]
        public async void AddRoute_RouteAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var repository = new RoutesRepository(context);
            
            // act
            await repository.AddRoute(new Route()
            {
                Id = Guid.NewGuid(),
                Name = "test route",
                LocationId = Guid.NewGuid(),
                SourceKey = Guid.NewGuid()
            });
            await repository.SaveChanges();
            
            // assert
            Assert.Single(context.Routes);
            Assert.Equal("test route", context.Routes.First().Name);
        }

        #endregion
    }
}