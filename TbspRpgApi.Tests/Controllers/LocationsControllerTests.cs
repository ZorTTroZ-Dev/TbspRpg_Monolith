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
    public class LocationsControllerTests : ApiTest
    {
        private static LocationsController CreateController(
            ICollection<Location> locations)
        {
            var locationsService = CreateLocationsService(locations);
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
    }
}