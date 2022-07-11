using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class GamesServiceTests : ApiTest
    {
        #region StartGame
        
        // these tests are kind of lame.
        [Fact]
        public async void StartGame_ExceptionThrown()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var service = CreateGamesService(new List<Game>(), exceptionId);
            
            // act
            Task Act() => service.StartGame(exceptionId, Guid.NewGuid(), DateTime.Now);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void StartGame_GameStarted()
        {
            // arrange
            var service = CreateGamesService(new List<Game>());
            
            // act
            await service.StartGame(Guid.NewGuid(), Guid.NewGuid(), DateTime.Now);
            
            // assert
            
        }

        #endregion

        #region GetGameByAdventureIdAndUserId

        [Fact]
        public async void GetGameByAdventureIdAndUserId_Exists_ReturnGameViewModel()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            var service = CreateGamesService(new List<Game>() {testGame});
            
            // act
            var gameViewModel = await service.GetGameByAdventureIdAndUserId(
                testGame.AdventureId, testGame.UserId);
            
            // assert
            Assert.NotNull(gameViewModel);
            Assert.Equal(testGame.Id, gameViewModel.Id);
        }

        [Fact]
        public async void GetGameByAdventureIdAndUserId_NotExist_ReturnNull()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            var service = CreateGamesService(new List<Game>() {testGame});
            
            // act
            var gameViewModel = await service.GetGameByAdventureIdAndUserId(
                Guid.NewGuid(), testGame.UserId);
            
            // assert
            Assert.Null(gameViewModel);
        }

        #endregion

        #region GetGames

        [Fact]
        public async void GetGames_ReturnsGames()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                User = new User()
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.com",
                    RegistrationComplete = true
                }
            };
            var service = CreateGamesService(new List<Game>() {testGame});
            
            // act
            var games = await service.GetGames(new GameFilterRequest()
            {
                AdventureId = testGame.AdventureId,
                UserId = testGame.UserId
            });
            
            // assert
            Assert.Single(games);
        }

        #endregion

        #region RemoveGame

        [Fact]
        public async void RemoveGame_InvalidGameId_ThrowException()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var service = CreateGamesService(new List<Game>(), exceptionId);
            
            // act
            Task Act() => service.DeleteGame(exceptionId);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void RemoveGame_GameRemoved()
        {
            // arrange
            var service = CreateGamesService(new List<Game>());
            
            // act
            await service.DeleteGame(Guid.NewGuid());
            
            // assert
            
        }

        #endregion

        #region GetGameState

        [Fact]
        public async void GetGameState_ValidGameId_JsonReturned()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                GameState = "{\"test\":\"value\"}"
            };
            var service = CreateGamesService(new List<Game>() {testGame});
            
            // act
            var state = await service.GetGameState(testGame.Id);
            
            // assert
            Assert.NotNull(state["test"]);
            Assert.Equal("value", state["test"].ToString());
        }

        [Fact]
        public async void GetGameState_InvalidGameId_ExceptionThrown()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                GameState = "{\"test\":\"value\"}"
            };
            var service = CreateGamesService(new List<Game>() {testGame});
            
            // act
            Task Act() => service.GetGameState(Guid.NewGuid());

            // assert
            await Assert.ThrowsAsync<NullReferenceException>(Act);
        }
        
        [Fact]
        public async void GetGameState_InvalidJson_ExceptionThrown()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                GameState = "{\"test':\"value\"}"
            };
            var service = CreateGamesService(new List<Game>() {testGame});
            
            // act
            Task Act() => service.GetGameState(testGame.Id);

            // assert
            await Assert.ThrowsAsync<System.Text.Json.JsonException>(Act);
        }

        #endregion
    }
}