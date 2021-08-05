using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;
using Xunit;

namespace TbspRpgDataLayer.Tests.Services
{
    public class ContentsServiceTests : InMemoryTest
    {
        public ContentsServiceTests() : base("ContentsServiceTests")
        {
        }

        private static IContentsService CreateService(DatabaseContext context)
        {
            return new ContentsService(new ContentsRepository(context),
                NullLogger<ContentsService>.Instance);
        }

        #region AddContent

        [Fact]
        public async void AddContent_NotExist_ContentAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Position = 42
            };
            var service = CreateService(context);
            
            // act
            service.AddContent(testContent);
            service.SaveChanges();
            
            // assert
            Assert.Single(context.Contents);
            Assert.Equal(testContent.Id, context.Contents.First().Id);
        }

        [Fact]
        public async void AddContent_Exists_ContentNotAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Position = 42
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            service.AddContent(testContent);
            service.SaveChanges();
            
            // assert
            Assert.Single(context.Contents);
            Assert.Equal(testContent.Id, context.Contents.First().Id);
        }

        #endregion

        #region GetContentForGameAtPosition

        [Fact]
        public async void GetContentForGameAtPosition_Exists_ReturnContent()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Position = 42
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var content = await service.GetContentForGameAtPosition(testContent.GameId, 42);
            
            // assert
            Assert.NotNull(content);
            Assert.Equal(testContent.Id, content.Id);
        }

        [Fact]
        public async void GetContentForGameAtPosition_NotExist_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                GameId = Guid.NewGuid(),
                Position = 42
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var content = await service.GetContentForGameAtPosition(Guid.NewGuid(), 42);
            
            // assert
            Assert.Null(content);
        }

        #endregion
    }
}