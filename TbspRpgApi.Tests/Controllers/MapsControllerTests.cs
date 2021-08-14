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
        private static MapsController CreateController(ICollection<Game> games)
        {
            var mapsService = CreateMapsService(games);
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
    }
}