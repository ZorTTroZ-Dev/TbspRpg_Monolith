using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
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
            return new LocationsController(locationsService,
                MockPermissionService(),
                NullLogger<LocationsController>.Instance);
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
                new LocationUpdateRequest()
                {
                    location = new LocationViewModel(new Location()
                    {
                        Id = Guid.NewGuid(),
                        SourceKey = Guid.NewGuid(),
                        AdventureId = Guid.NewGuid(),
                        Name = "test location",
                        Initial = true,
                        AdventureObjects = new List<AdventureObject>()
                    }),
                    source = new SourceViewModel(Guid.Empty, "test source")
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }

        [Fact]
        public async void UpdateLocationAndSource_UpdateFails_ReturnBadRequest()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var controller = CreateController(new List<Location>(), exceptionId);
            
            // act
            var response = await controller.UpdateLocationAndSource(
                new LocationUpdateRequest()
                {
                    location = new LocationViewModel(new Location()
                    {
                        Id = exceptionId,
                        SourceKey = Guid.NewGuid(),
                        AdventureId = Guid.NewGuid(),
                        Name = "test location",
                        Initial = true
                    }),
                    source = new SourceViewModel(Guid.Empty, "test source")
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion

        #region GetLocationById

        [Fact]
        public async void GetLocationById_ReturnsLocation()
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
            var response = await controller.GetLocationById(testLocations[0].Id);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var locationViewModel = okObjectResult.Value as LocationViewModel;
            Assert.NotNull(locationViewModel);
            Assert.Equal("test", locationViewModel.Name);
        }

        #endregion

        #region RemoveLocation

        [Fact]
        public async void RemoveLocation_InvalidId_BadRequest()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var controller = CreateController(new List<Location>(), exceptionId);
            
            // act
            var response = await controller.DeleteLocation(exceptionId);
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void RemoveLocation_Valid_ReturnOk()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var controller = CreateController(new List<Location>(), exceptionId);
            
            // act
            var response = await controller.DeleteLocation(Guid.NewGuid());
            
            // assert
            var okObjectResult = response as OkResult;
            Assert.NotNull(okObjectResult);
        }

        #endregion
    }
}