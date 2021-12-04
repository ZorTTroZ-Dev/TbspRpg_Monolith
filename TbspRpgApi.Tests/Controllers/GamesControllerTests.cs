using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
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
            var service = CreateGamesService(games, startGameExceptionId);
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

        [Fact]
        public async void StartGame_Valid_Accepted()
        {
            // arrange
            var userId = Guid.NewGuid();
            var controller = CreateGamesController(new List<Game>(), Guid.NewGuid(), userId);
            
            // act
            var response = await controller.StartGame(Guid.NewGuid());
            
            // assert
            var acceptedResult = response as AcceptedResult;
            Assert.NotNull(acceptedResult);
            Assert.Equal(202, acceptedResult.StatusCode);
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
                UserId = userId
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
            var gameViewModels = okObjectResult.Value as List<GameViewModel>;
            Assert.NotNull(gameViewModels);
            Assert.Equal(testGame.Id, gameViewModels[0].Id);
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
            var gameViewModels = okObjectResult.Value as List<GameViewModel>;
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
    }
}