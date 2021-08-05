using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories
{
    public class ContentsRepositoryTests : InMemoryTest
    {
        public ContentsRepositoryTests() : base("ContentsRepositoryTests")
        {
        }

        private static IContentsRepository CreateRepository(DatabaseContext context)
        {
            return new ContentsRepository(context);
        }

        #region AddContent

        [Fact]
        public async void AddContent_ContentAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                Position = 42
            };
            var repository = CreateRepository(context);
            
            // act
            repository.AddContent(testContent);
            repository.SaveChanges();
            
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
                Position = 42,
                GameId = Guid.NewGuid()
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var repository = CreateRepository(context);
            
            // act
            var content = await repository.GetContentForGameAtPosition(testContent.GameId, 42);
            
            // assert
            Assert.NotNull(content);
            Assert.Equal(testContent.Id, content.Id);
        }
        
        [Fact]
        public async void GetContentForGameAtPosition_InvalidGame_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                Position = 42,
                GameId = Guid.NewGuid()
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var repository = CreateRepository(context);
            
            // act
            var content = await repository.GetContentForGameAtPosition(Guid.NewGuid(), 42);
            
            // assert
            Assert.Null(content);
        }
        
        [Fact]
        public async void GetContentForGameAtPosition_InvalidPosition_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testContent = new Content()
            {
                Id = Guid.NewGuid(),
                Position = 42,
                GameId = Guid.NewGuid()
            };
            context.Contents.Add(testContent);
            await context.SaveChangesAsync();
            var repository = CreateRepository(context);
            
            // act
            var content = await repository.GetContentForGameAtPosition(testContent.GameId, 40);
            
            // assert
            Assert.Null(content);
        }

        #endregion
    }
}