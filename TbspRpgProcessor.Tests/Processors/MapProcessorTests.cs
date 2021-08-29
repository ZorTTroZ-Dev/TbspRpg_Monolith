using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class MapProcessorTests: ProcessorTest
    {
        #region ChangeLocationViaRoute

        [Fact]
        public async void ChangeLocationViaRoute_InvalidGameId_ThrowsExcpetion()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                SuccessSourceKey = Guid.NewGuid()
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = testRoute.LocationId
                }
            };
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                new List<Content>());
            
            // act
            Task Act() => processor.ChangeLocationViaRoute(Guid.NewGuid(),
                testRoute.Id, DateTime.UtcNow);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void ChangeLocationViaRoute_InvalidRouteId_ThrowsExcpetion()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                SuccessSourceKey = Guid.NewGuid()
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = testRoute.LocationId
                }
            };
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                new List<Content>());
            
            // act
            Task Act() => processor.ChangeLocationViaRoute(
                testGames[0].Id,
                Guid.NewGuid(), 
                DateTime.UtcNow);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void ChangeLocationViaRoute_GameWrongLocation_ThrowsExcpetion()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                SuccessSourceKey = Guid.NewGuid()
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid()
                }
            };
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                new List<Content>());
            
            // act
            Task Act() => processor.ChangeLocationViaRoute(
                testGames[0].Id,
                testRoute.Id,
                DateTime.UtcNow);

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }
        
        [Fact]
        public async void ChangeLocationViaRoute_Valid_LocationUpdated()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                SuccessSourceKey = Guid.NewGuid()
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = testRoute.LocationId
                }
            };
            var testContents = new List<Content>();
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                testContents);
            
            // act
            await processor.ChangeLocationViaRoute(
                testGames[0].Id,
                testRoute.Id,
                DateTime.UtcNow);
            
            // assert
            var game = testGames[0];
            Assert.Equal(testRoute.DestinationLocationId, game.LocationId);
            Assert.True(game.LocationUpdateTimeStamp > 0);
            Assert.Single(testContents);
            Assert.Equal(testContents[0].SourceKey, testRoute.SuccessSourceKey);
        }

        #endregion
    }
}