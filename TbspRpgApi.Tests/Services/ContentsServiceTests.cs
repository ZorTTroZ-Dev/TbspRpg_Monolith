using System;
using System.Collections.Generic;
using TbspRpgApi.Entities;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgApi.RequestModels;
using TbspRpgDataLayer.Entities;
using TbspRpgSettings.Settings;
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
            var service = CreateContentsService(testContents);
            
            // act
            var contentViewModel = await service.GetLatestForGame(testGameId);
            
            // assert
            Assert.NotNull(contentViewModel);
            Assert.Single(contentViewModel.SourceKeys);
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
            var service = CreateContentsService(testContents);
            
            // act
            var contentViewModel = await service.GetLatestForGame(Guid.NewGuid());
            
            // assert
            Assert.Null(contentViewModel);
        }

        #endregion

        #region GetPartialContentForGame

        [Fact]
        public async void GetPartialContentForGame_NoContent_ReturnNull()
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
            var service = CreateContentsService(testContents);
            
            // act
            var contents = await service.GetPartialContentForGame(
                testGameId,
                new ContentFilterRequest()
                {
                    Direction = "f",
                    Start = 3,
                    Count = 1
                });
            
            // assert
            Assert.Null(contents);
        }
        
        [Fact]
        public async void GetPartialContentForGame_Content_ReturnContent()
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
            var service = CreateContentsService(testContents);
            
            // act
            var contents = await service.GetPartialContentForGame(
                testGameId,
                new ContentFilterRequest()
                {
                    Direction = "f",
                    Start = 0,
                    Count = 1
                });
            
            // assert
            Assert.NotNull(contents);
            Assert.Single(contents.SourceKeys);
            Assert.Equal(testGameId, contents.Id);
            Assert.Equal((ulong)0, contents.Index);
        }

        #endregion

        #region GetContentForGameAfterPosition

        [Fact]
        public async void GetContentForGameAfterPosition_NoResults_ReturnNull()
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
            var service = CreateContentsService(testContents);
            
            //act
            var contentViewModel = await service.GetContentForGameAfterPosition(testGameId, 43);
            
            // assert
            Assert.Null(contentViewModel);
        }
        
        [Fact]
        public async void GetContentForGameAfterPosition_ReturnContents()
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
            var service = CreateContentsService(testContents);
            
            // act
            var contentViewModel = await service.GetContentForGameAfterPosition(testGameId, 10);
            
            // assert
            Assert.NotNull(contentViewModel);
            Assert.Equal(testGameId, contentViewModel.Id);
            Assert.Single(contentViewModel.SourceKeys);
            Assert.Equal((ulong)42, contentViewModel.Index);
        }

        #endregion

        #region GetContentTextForKey

        [Fact]
        public async void GetContentTextForKey_BadKey_ReturnNull()
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
            var service = CreateContentsService(
                new List<Content>(),
                new List<Game>() { testGame },
                new List<En>() { testSource });
            
            // act
            var source = await service.GetContentTextForKey(testGame.Id, Guid.NewGuid());
            
            // assert
            Assert.Null(source);
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
            var service = CreateContentsService(
                new List<Content>(),
                new List<Game>() { testGame },
                new List<En>() { testSource });
            
            // act
            var sourceViewModel = await service.GetContentTextForKey(testGame.Id, testSource.Key);
            
            // assert
            Assert.NotNull(sourceViewModel);
            Assert.Equal("test source", sourceViewModel.Text);
        }

        

        #endregion
        
        #region GetProcessedContentTextForKey

        [Fact]
        public async void GetProcessedContentTextForKey_BadKey_ReturnNull()
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
            var service = CreateContentsService(
                new List<Content>(),
                new List<Game>() { testGame },
                new List<En>() { testSource });
            
            // act
            var source = await service.GetProcessedContentTextForKey(testGame.Id, Guid.NewGuid());
            
            // assert
            Assert.Null(source);
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
            var service = CreateContentsService(
                new List<Content>(),
                new List<Game>() { testGame },
                new List<En>() { testSource });
            
            // act
            var sourceViewModel = await service.GetProcessedContentTextForKey(testGame.Id, testSource.Key);
            
            // assert
            Assert.NotNull(sourceViewModel);
            Assert.Equal("<p>test source</p>", sourceViewModel.Text);
        }

        

        #endregion
    }
}