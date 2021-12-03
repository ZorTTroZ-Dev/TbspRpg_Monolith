using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;
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

        #region GetCurrentRoutesForGame

        [Fact]
        public async void GetCurrentRoutesForGame_Valid_ReturnRoutes()
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
            var service = CreateMapsService(new List<Game>() {testGame}, testRoutes);
            
            // act
            var routes = await service.GetCurrentRoutesForGame(testGame.Id);
            
            // assert
            Assert.Equal(2, routes.Count);
            Assert.NotNull(routes.FirstOrDefault(r => r.Name == "route1"));
        }
        
        [Fact]
        public async void GetCurrentRoutesForGame_NoGame_ThrowException()
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
            var service = CreateMapsService(new List<Game>() {testGame}, testRoutes);
            
            // act
            Task Act() => service.GetCurrentRoutesForGame(Guid.NewGuid());

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }
        
        [Fact]
        public async void GetCurrentRoutesForGame_NoLocation_ThrowException()
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
            var service = CreateMapsService(new List<Game>() {testGame}, testRoutes);
            
            // act
            Task Act() => service.GetCurrentRoutesForGame(testGame.Id);

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }

        #endregion

        #region GetCurrentRoutesForGameAfterTimeStamp

        [Fact]
        public async void GetCurrentRoutesForGameAfterTimeStamp_NewRoutes_ReturnRoutes()
        {
            // arrange
            var testLocationId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocationId,
                LocationUpdateTimeStamp = 42
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
            var service = CreateMapsService(new List<Game>() {testGame}, testRoutes);
            
            // act
            var routes = 
                await service.GetCurrentRoutesForGameAfterTimeStamp(testGame.Id, 15);
            
            // assert
            Assert.Equal(2, routes.Count);
            Assert.NotNull(routes.FirstOrDefault(r => r.Name == "route1"));
        }
        
        [Fact]
        public async void GetCurrentRoutesForGameAfterTimeStamp_NoNewRoutes_ReturnEmpty()
        {
            // arrange
            var testLocationId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocationId,
                LocationUpdateTimeStamp = 42
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
            var service = CreateMapsService(new List<Game>() {testGame}, testRoutes);
            
            // act
            var routes = 
                await service.GetCurrentRoutesForGameAfterTimeStamp(testGame.Id, 50);
            
            // assert
            Assert.Empty(routes);
        }

        #endregion

        #region ChangeLocationViaRoute

        [Fact]
        public async void ChangeLocationViaRoute_BadArgument_ThrowException()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var service = CreateMapsService(null, null, exceptionId);
            
            // act
            Task Act() => service.ChangeLocationViaRoute(exceptionId, Guid.NewGuid(), DateTime.UtcNow);
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        #endregion
    }
}