using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services
{
    public class ContentsServiceTests : ApiTest
    {
        #region GetLatestForGame

        [Fact]
        public async void GetLatestForGame_ReturnsLatest()
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
            var service = CreateContentsService(testContents);
            
            // act
            var contentViewModel = await service.GetLatestForGame(testGameId);
            
            // assert
            Assert.NotNull(contentViewModel);
            Assert.Single(contentViewModel.SourceIds);
            Assert.Equal(testGameId, contentViewModel.Id);
            Assert.Equal((ulong)42, contentViewModel.Index);
        }
        
        [Fact]
        public async void GetLatestForGame_InvalidGameId_ReturnNull()
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
            var service = CreateContentsService(testContents);
            
            // act
            var contentViewModel = await service.GetLatestForGame(Guid.NewGuid());
            
            // assert
            Assert.Null(contentViewModel);
        }

        #endregion
    }
}