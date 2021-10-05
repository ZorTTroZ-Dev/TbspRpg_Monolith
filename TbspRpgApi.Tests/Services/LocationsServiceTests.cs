using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
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

        #region UpdateLocation

        [Fact]
        public async void UpdateLocation_Valid_LocationUpdated()
        {
            // arrange
            var service = CreateLocationsService(new List<Location>(), Guid.NewGuid());
            // act
            await service.UpdateLocationAndSource(new UpdateLocationRequest()
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
        }
        
        [Fact]
        public async void UpdateLocation_InValid_ExceptionThrown()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var service = CreateLocationsService(new List<Location>(), exceptionId);
            
            // act
            Task Act() => service.UpdateLocationAndSource(new UpdateLocationRequest()
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
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion
    }
}