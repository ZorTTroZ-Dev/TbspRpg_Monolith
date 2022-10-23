using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class GamesControllerTests : ApiTest
    {
        private static GamesController CreateGamesController(
            ICollection<Game> games, Guid startGameExceptionId, Guid? userId)
        {
            var service = CreateGamesService(games, 
                new List<Route>(), new List<Content>(), new List<En>(), startGameExceptionId);
            var controller = new GamesController(service,
                MockPermissionService(),
                NullLogger<GamesController>.Instance)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
            
            controller.ControllerContext.HttpContext.Items = new Dictionary<object, object>()
            {
                { AuthorizeAttribute.USER_ID_CONTEXT_KEY, userId }
            };
            return controller;
        }

        #region StartGame

        [Fact]
        public async void StartGame_NoUser_BadRequest()
        {
            // arrange
            var controller = CreateGamesController(new List<Game>(), Guid.NewGuid(), null);
            
            // act
            var response = await controller.StartGame(Guid.NewGuid());
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void StartGame_ExceptionThrown_BadRequest()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var controller = CreateGamesController(new List<Game>(), exceptionId, exceptionId);
            
            // act
            var response = await controller.StartGame(Guid.NewGuid());
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion

        #region GetGameByAdventure

        [Fact]
        public async void GetGameByAdventure_Exist_ReturnGame()
        {
            // arrange
            var userId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                UserId = userId
            };
            var controller = CreateGamesController(new List<Game>()
            {
                testGame
            }, Guid.NewGuid(), userId);
            
            // act
            var response = await controller.GetGameByAdventure(testGame.AdventureId);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var gameViewModel = okObjectResult.Value as GameViewModel;
            Assert.NotNull(gameViewModel);
            Assert.Equal(testGame.Id, gameViewModel.Id);
        }

        [Fact]
        public async void GetGameByAdventure_NotExist_EmptyResponse()
        {
            // arrange
            var userId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                UserId = userId
            };
            var controller = CreateGamesController(new List<Game>()
            {
                testGame
            }, Guid.NewGuid(), userId);
            
            // act
            var response = await controller.GetGameByAdventure(Guid.NewGuid());
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var gameViewModel = okObjectResult.Value as GameViewModel;
            Assert.Null(gameViewModel);
        }

        [Fact]
        public async void GetGameByAdventure_NoUser_BadRequest()
        {
            // arrange
            var controller = CreateGamesController(new List<Game>(), Guid.NewGuid(), null);
            
            // act
            var response = await controller.GetGameByAdventure(Guid.NewGuid());
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion

        #region GetGames

        [Fact]
        public async void GetGames_ReturnsGames()
        {
            // arrange
            var userId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                UserId = userId,
                User = new User()
                {
                    Id = userId,
                    Email = "test@test.com",
                    RegistrationComplete = true
                }
            };
            var controller = CreateGamesController(new List<Game>()
            {
                testGame
            }, Guid.NewGuid(), userId);
            
            // act
            var response = await controller.GetGames(new GameFilterRequest()
            {
                AdventureId = testGame.AdventureId,
                UserId = userId
            });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var gameViewModels = okObjectResult.Value as List<GameUserViewModel>;
            Assert.NotNull(gameViewModels);
            Assert.Equal(testGame.Id, gameViewModels[0].Game.Id);
        }
        
        [Fact]
        public async void GetGames_NoGames_ReturnsEmptyList()
        {
            // arrange
            var userId = Guid.NewGuid();
            var controller = CreateGamesController(new List<Game>(){}, Guid.NewGuid(), userId);
            
            // act
            var response = await controller.GetGames(new GameFilterRequest()
            {
                AdventureId = Guid.NewGuid(),
                UserId = userId
            });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var gameViewModels = okObjectResult.Value as List<GameUserViewModel>;
            Assert.NotNull(gameViewModels);
            Assert.Empty(gameViewModels);
        }

        #endregion
        
        #region DeleteGame

        [Fact]
        public async void DeleteGame_ExceptionThrown_BadRequest()
        {
            // arrange
            var exceptionId = Guid.NewGuid();
            var controller = CreateGamesController(new List<Game>(), exceptionId, exceptionId);
            
            // act
            var response = await controller.DeleteGame(exceptionId);
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void DeleteGame_Valid_Accepted()
        {
            // arrange
            var userId = Guid.NewGuid();
            var controller = CreateGamesController(new List<Game>(), Guid.NewGuid(), userId);
            
            // act
            var response = await controller.DeleteGame(Guid.NewGuid());
            
            // assert
            var okResult = response as OkResult;
            Assert.NotNull(okResult);
            Assert.Equal(200, okResult.StatusCode);
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
            var controller = CreateGamesController(new List<Game>() {testGame}, Guid.Empty, null);
            
            // act
            var response = await controller.GetGameState(testGame.Id);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var jsonState = okObjectResult.Value as JsonObject;
            Assert.NotNull(jsonState);
            Assert.NotNull(jsonState["test"]);
            Assert.Equal("value", jsonState["test"].ToString());
        }

        [Fact]
        public async void GetGameState_InvalidGameId_BadRequest()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                GameState = "{\"test\":\"value\"}"
            };
            var controller = CreateGamesController(new List<Game>() {testGame}, Guid.Empty, null);
            
            // act
            var response = await controller.GetGameState(Guid.NewGuid());

            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        [Fact]
        public async void GetGameState_InvalidJson_BadRequest()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                GameState = "{\"test':\"value\"}"
            };
            var controller = CreateGamesController(new List<Game>() {testGame}, Guid.Empty, null);
            
            // act
            var response = await controller.GetGameState(testGame.Id);

            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        #endregion
        
        #region UpdateGameState

        [Fact]
        public async void UpdateGameState_ValidGameId_GameStateUpdated()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                GameState = "{\"test\":\"value\"}"
            };
            var controller = CreateGamesController(new List<Game>() {testGame}, Guid.Empty, null);
            
            // act
            var response = await controller.UpdateGameState(new GameStateUpdateRequest()
            {
                GameId = testGame.Id,
                GameState = "{\"test\":\"banana\"}"
            });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
        }

        [Fact]
        public async void UpdateGameState_InvalidGameId_BadRequest()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                GameState = "{\"test\":\"value\"}"
            };
            var controller = CreateGamesController(new List<Game>() {testGame}, Guid.Empty, null);
            
            // act
            var response = await controller.UpdateGameState(new GameStateUpdateRequest()
            {
                GameId = Guid.NewGuid(),
                GameState = "{\"test\":\"banana\"}"
            });

            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
        
        #endregion
    }
}