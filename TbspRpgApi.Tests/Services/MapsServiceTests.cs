using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class MapsServiceTests : ApiTest
    {
        #region GetCurrentLocationForGame

        [Fact]
        public async void GetCurrentLocationForGame_ReturnLocation()
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
            var service = CreateMapsService(new List<Game>() { testGame });
            
            // act
            var location = await service.GetCurrentLocationForGame(testGame.Id);
            
            // assert
            Assert.NotNull(location);
            Assert.Equal(testLocationId, location.Id);
        }

        [Fact]
        public async void GetCurrentLocationForGame_NoGame_ThrowException()
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
            var service = CreateMapsService(new List<Game>() { testGame });
            
            // act
            Task Act() => service.GetCurrentLocationForGame(Guid.NewGuid());

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }

        [Fact]
        public async void GetCurrentLocationForGame_NoLocation_ThrowException()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            var service = CreateMapsService(new List<Game>() { testGame });
            
            // act
            Task Act() => service.GetCurrentLocationForGame(testGame.Id);
            
            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }

        #endregion
    }
}