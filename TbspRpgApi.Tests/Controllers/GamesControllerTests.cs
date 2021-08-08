using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.JwtAuthorization;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class GamesControllerTests : ApiTest
    {
        private static GamesController CreateGamesController(
            ICollection<Game> games, Guid startGameExceptionId, Guid? userId)
        {
            var service = CreateGamesService(games, startGameExceptionId);
            var controller = new GamesController(service, NullLogger<GamesController>.Instance)
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
    }
}