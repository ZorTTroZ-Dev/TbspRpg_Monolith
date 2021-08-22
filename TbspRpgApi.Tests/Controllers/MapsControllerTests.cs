using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.ViewModels;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class MapsControllerTests : ApiTest
    {
        private static MapsController CreateController(ICollection<Game> games, ICollection<Route> routes = null)
        {
            var mapsService = CreateMapsService(games, routes);
            return new MapsController(mapsService, NullLogger<MapsController>.Instance);
        }

        #region GetCurrentLocationForGame

        [Fact]
        public async void GetCurrentLocationForGame_NoGame_BadRequest()
        {
            // arrange
            var testLocationId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                Location = new Location()
                {
                    Id = testLocationId,
                    Name = "test location",
                    Initial = true
                }
            };
            var controller = CreateController(new List<Game>() {testGame});
            
            // act
            var response = await controller.GetCurrentLocationForGame(Guid.NewGuid());
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetCurrentLocationForGame_NoLocation_BadRequest()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            var controller = CreateController(new List<Game>() {testGame});
            
            // act
            var response = await controller.GetCurrentLocationForGame(testGame.Id);
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetCurrentLocationForGame_Valid_ReturnLocation()
        {
            // arrange
            var testLocationId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                Location = new Location()
                {
                    Id = testLocationId,
                    Name = "test location",
                    Initial = true
                }
            };
            var controller = CreateController(new List<Game>() {testGame});
            
            // act
            var response = await controller.GetCurrentLocationForGame(testGame.Id);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var locationViewModel = okObjectResult.Value as LocationViewModel;
            Assert.NotNull(locationViewModel);
            Assert.Equal(testLocationId, locationViewModel.Id);
        }

        #endregion

        #region GetCurrentRoutesForGame

        [Fact]
        public async void GetCurrentRoutesForGame_Valid_ReturnsRoutes()
        {
            // arrange
            var testLocationId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocationId
            };
            var testRoutes = new List<Route>()
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
            };
            var controller = CreateController(new List<Game>() {testGame}, testRoutes);
            
            // act
            var response = await controller.GetCurrentRoutesForGame(testGame.Id);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var routeViewModels = okObjectResult.Value as List<RouteViewModel>;
            Assert.NotNull(routeViewModels);
            Assert.Equal(2, routeViewModels.Count);
        }
        
        [Fact]
        public async void GetCurrentRoutesForGame_NoGame_BadRequest()
        {
            // arrange
            var testLocationId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            var testRoutes = new List<Route>()
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
            };
            var controller = CreateController(new List<Game>() {testGame}, testRoutes);
            
            // act
            var response = await controller.GetCurrentRoutesForGame(Guid.NewGuid());
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetCurrentRoutesForGame_NoLocation_BadRequest()
        {
            // arrange
            var testLocationId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            var testRoutes = new List<Route>()
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
            };
            var controller = CreateController(new List<Game>() {testGame}, testRoutes);
            
            // act
            var response = await controller.GetCurrentRoutesForGame(testGame.Id);
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion
    }
}