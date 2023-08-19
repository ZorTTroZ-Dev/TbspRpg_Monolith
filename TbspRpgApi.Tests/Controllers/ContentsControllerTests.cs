using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgApi.Tests.Controllers
{
    public class ContentsControllerTests : ApiTest
    {
        private static ContentsController CreateController(
            ICollection<Content> contents = null,
            ICollection<Game> games = null,
            ICollection<En> sources = null)
        {
            var service = CreateContentsService(contents, Guid.Empty, games, sources);
            return new ContentsController(service,
                MockPermissionService(),
                NullLogger<ContentsController>.Instance);
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
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
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
            Assert.Single(contentViewModel.SourceKeys);
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
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
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
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
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
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
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
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
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
            Assert.Single(contentViewModel.SourceKeys);
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
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGameId,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
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
            Assert.Single(contentViewModel.SourceKeys);
        }

        #endregion
        
        #region GetContentTextForKey

        [Fact]
        public async void GetContentTextForKey_InvalidKey_BadRequest()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
            };
            var controller = CreateController(
                null,
                new List<Game>() {testGame},
                new List<En>());
            
            // act
            var response = await controller.GetContentTextForKey(testGame.Id, Guid.NewGuid());
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void GetContentTextForKey_ValidKey_ReturnSource()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Text = "test source"
            };
            var controller = CreateController(
                null,
                new List<Game>() {testGame},
                new List<En>() {testSource});
            
            // act
            var response = await controller.GetContentTextForKey(testGame.Id, testSource.Key);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var source = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(source);
            Assert.Equal("test source", source.Text);
        }

        #endregion
        
        #region GetProcessedContentTextForKey

        [Fact]
        public async void GetProcessedContentTextForKey_InvalidKey_BadRequest()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
            };
            var controller = CreateController(
                null,
                new List<Game>() {testGame},
                new List<En>());
            
            // act
            var response = await controller.GetProcessedContentTextForKey(testGame.Id, Guid.NewGuid());
            
            // assert
            var badRequestResult = response as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }

        [Fact]
        public async void GetProcessedContentTextForKey_ValidKey_ReturnSource()
        {
            // arrange
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
            };
            var testSource = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                Text = "test source"
            };
            var controller = CreateController(
                null,
                new List<Game>() {testGame},
                new List<En>() {testSource});
            
            // act
            var response = await controller.GetProcessedContentTextForKey(testGame.Id, testSource.Key);
            
            // assert
            var okObjectResult = response as OkObjectResult;
            Assert.NotNull(okObjectResult);
            var source = okObjectResult.Value as SourceViewModel;
            Assert.NotNull(source);
            Assert.Equal("test source", source.Text);
        }

        #endregion
    }
}