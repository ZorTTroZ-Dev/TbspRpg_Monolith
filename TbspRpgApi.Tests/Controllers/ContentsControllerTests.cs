using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class ContentsControllerTests : ApiTest
    {
        private static ContentsController CreateController(ICollection<Content> contents)
        {
            var service = CreateContentsService(contents);
            return new ContentsController(service, NullLogger<ContentsController>.Instance);
        }
        
        #region GetLatestContentForGame

        [Fact]
        public async void GetLatestContentForGame_ReturnsLatest()
        {
            // arrange
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42,
                    SourceId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testContents);
            
            // act
            var response = await controller.GetLatestContentForGame(testGameId);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var contentViewModel = okObjectResult.Value as ContentViewModel;
            Assert.NotNull(contentViewModel);
            Assert.Equal(testGameId, contentViewModel.Id);
            Assert.Single(contentViewModel.SourceIds);
        }
        
        [Fact]
        public async void GetLatestContentForGame_InvalidGameId_ReturnsEmpty()
        {
            // arrange
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42,
                    SourceId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testContents);
            
            // act
            var response = await controller.GetLatestContentForGame(Guid.NewGuid());
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var contentViewModel = okObjectResult.Value as ContentViewModel;
            Assert.Null(contentViewModel);
        }

        #endregion

        #region GetPartialContentForGame

        [Fact]
        public async void GetPartialContentForGame_ExceptionThrown_BadRequest()
        {
            // arrange
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42,
                    SourceId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testContents);
            
            // act
            var response = await controller.GetPartialContentForGame(
                testGameId,
                new ContentFilterRequest()
                {
                    Direction = "z",
                    Start = 0,
                    Count = 2
                });
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void GetPartialContentForGame_NoContent_EmptyResponse()
        {
            // arrange
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42,
                    SourceId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testContents);
            
            // act
            var response = await controller.GetPartialContentForGame(
                testGameId,
                new ContentFilterRequest()
                {
                    Direction = "f",
                    Start = 3,
                    Count = 2
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var contentViewModel = okObjectResult.Value as ContentViewModel;
            Assert.Null(contentViewModel);
        }

        [Fact]
        public async void GetPartialContentForGame_Valid_ReturnContent()
        {
            // arrange
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42,
                    SourceId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testContents);
            
            // act
            var response = await controller.GetPartialContentForGame(
                testGameId,
                new ContentFilterRequest()
                {
                    Direction = "f",
                    Start = 1,
                    Count = 1
                });
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var contentViewModel = okObjectResult.Value as ContentViewModel;
            Assert.NotNull(contentViewModel);
            Assert.Equal(testGameId, contentViewModel.Id);
            Assert.Single(contentViewModel.SourceIds);
        }

        #endregion

        #region GetContentForGameAfterPosition

        [Fact]
        public async void GetContentForGameAfterPosition_ReturnsContent()
        {
            // arrange
            var testGameId = Guid.NewGuid();
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 42,
                    SourceId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceId = Guid.NewGuid()
                }
            };
            var controller = CreateController(testContents);
            
            // act
            var response = await controller.GetContentForGameAfterPosition(testGameId, 10);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var contentViewModel = okObjectResult.Value as ContentViewModel;
            Assert.NotNull(contentViewModel);
            Assert.Equal(testGameId, contentViewModel.Id);
            Assert.Single(contentViewModel.SourceIds);
        }

        #endregion
    }
}