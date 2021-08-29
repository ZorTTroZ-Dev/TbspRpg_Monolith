using System;
using System.Collections.Generic;
using Castle.Core;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
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
    }
}