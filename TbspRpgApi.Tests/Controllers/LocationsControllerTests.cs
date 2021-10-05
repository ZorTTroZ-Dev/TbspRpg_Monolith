using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class LocationsControllerTests : ApiTest
    {
        private static LocationsController CreateController(
            ICollection<Location> locations,
            Guid? updateLocationExceptionId = null)
        {
            var locationsService = CreateLocationsService(locations, updateLocationExceptionId);
            return new LocationsController(locationsService, NullLogger<LocationsController>.Instance);
        }

        #region GetLocationsForAdventure

        [Fact]
        public async void GetLocationsForAdventure_ReturnLocations()
        {
            // arrange
            var testLocations = new List<Location>()
            {
                new Location()
                {
                    AdventureId = Guid.NewGuid(),
                    Id = Guid.NewGuid(),
                    Name = "test"
                }
            };
            var controller = CreateController(testLocations);
            
            // act
            var response = await controller.GetLocationsForAdventure(testLocations[0].AdventureId);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var locationViewModels = okObjectResult.Value as List<LocationViewModel>;
            Assert.NotNull(locationViewModels);
            Assert.Single(locationViewModels);
        }

        #endregion

        #region UpdateLocationAndSource

        [Fact]
        public async void UpdateLocationAndSource_Valid_ReturnOk()
        {
            // arrange
            var controller = CreateController(new List<Location>(), Guid.NewGuid());
            
            // act
            var response = await controller.UpdateLocationAndSource(
                new UpdateLocationRequest()
                {
                    location = new LocationViewModel(new Location()
                    {
                        Id = Guid.NewGuid(),
                        SourceKey = Guid.NewGuid(),
                        AdventureId = Guid.NewGuid(),
                        Name = "test location",
                        Initial = true
                    }),
                    source = new SourceViewModel("test source")
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.Null(okObjectResult);
        }

        [Fact]
        public async void UpdateLocationAndSource_UpdateFails_ReturnBadRequest()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var controller = CreateController(new List<Location>(), exceptionId);
            
            // act
            var response = await controller.UpdateLocationAndSource(
                new UpdateLocationRequest()
                {
                    location = new LocationViewModel(new Location()
                    {
                        Id = exceptionId,
                        SourceKey = Guid.NewGuid(),
                        AdventureId = Guid.NewGuid(),
                        Name = "test location",
                        Initial = true
                    }),
                    source = new SourceViewModel("test source")
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion
    }
}