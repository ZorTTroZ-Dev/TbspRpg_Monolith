using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class LocationsServiceTests : ApiTest
    {
        #region GetLocationsForAdventure

        [Fact]
        public async void GetLocationsForAdventure_ReturnsLocations()
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
            var service = CreateLocationsService(testLocations);
            
            // act
            var locations = await service.GetLocationsForAdventure(testLocations[0].AdventureId);
            
            // assert
            Assert.Single(locations);
            Assert.Equal("test", locations[0].Name);
        }

        #endregion
    }
}