using System;
using System.Collections.Generic;
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
                testAdventures,
                new List<Game>());
            
            // act
            Task Act() => processor.StartGame(Guid.NewGuid(), testAdventures[0].Id);

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
                testAdventures,
                new List<Game>());
            
            // act
            Task Act() => processor.StartGame(testUsers[0].Id, Guid.NewGuid());

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void StartGame_GameExists_ThrowsException()
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
            Task Act() => processor.StartGame(testUsers[0].Id, testAdventures[0].Id);

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }

        #endregion
    }
}