using System;
using System.Data.Common;
using System.Linq;
using TbspRpgApi.Entities;
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
    }
}