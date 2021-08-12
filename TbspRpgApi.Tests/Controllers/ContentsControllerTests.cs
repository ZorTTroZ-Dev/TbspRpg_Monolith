using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.Entities;
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
    }
}