using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class GameProcessorTests : ProcessorTest
    {
        #region StartGame

        [Fact]
        public async void StartGame_InvalidUserId_ThrowsException()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserName = "test"
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures);
            
            // act
            Task Act() => processor.StartGame(Guid.NewGuid(),
                testAdventures[0].Id, DateTime.Now);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void StartGame_InvalidAdventureId_ThrowsException()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserName = "test"
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures);
            
            // act
            Task Act() => processor.StartGame(testUsers[0].Id,
                Guid.NewGuid(), DateTime.Now);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void StartGame_GameExists_ReturnsGame()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserName = "test"
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    UserId = testUsers[0].Id
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                testGames);

            // act
            var game = await processor.StartGame(testUsers[0].Id,
                testAdventures[0].Id, DateTime.Now);

            // assert
            Assert.Single(testGames);
            Assert.NotNull(game);
            Assert.Equal(testAdventures[0].Id, game.AdventureId);
            Assert.Equal(testUsers[0].Id, game.UserId);
        }
        
        [Fact]
        public async void StartGame_LocationDoesntExist_ThrowsException()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserName = "test"
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                new List<Game>(),
                new List<Location>());
            
            // act
            Task Act() => processor.StartGame(testUsers[0].Id,
                testAdventures[0].Id, DateTime.Now);

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }
        
        [Fact]
        public async void StartGame_Valid_GameCreated()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    SourceKey = Guid.NewGuid()
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    UserName = "test"
                }
            };
            var testLocations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    Initial = true,
                    SourceKey = Guid.NewGuid()
                }
            };
            var testContents = new List<Content>();
            var testGames = new List<Game>();
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                testGames,
                testLocations,
                testContents);
            
            // act
            var game = await processor.StartGame(testUsers[0].Id,
                testAdventures[0].Id, DateTime.Now);

            // assert
            Assert.Single(testGames);
            Assert.NotNull(game);
            Assert.Equal(testAdventures[0].Id, game.AdventureId);
            Assert.Equal(testUsers[0].Id, game.UserId);
            Assert.Equal(testLocations[0].Id, game.LocationId);
            Assert.Equal(2, testContents.Count);
            Assert.NotNull(testContents.FirstOrDefault(c => c.SourceKey == testAdventures[0].SourceKey));
            Assert.NotNull(testContents.FirstOrDefault(c => c.SourceKey == testLocations[0].SourceKey));
        }

        #endregion
    }
}