using System;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;
using Xunit;

namespace TbspRpgDataLayer.Tests.Services
{
    public class GamesServiceTests : InMemoryTest
    {
        public GamesServiceTests() : base("GamesServiceTests")
        {
        }
        
        private static IGamesService CreateService(DatabaseContext context)
        {
            return new GamesService(
                new GameRepository(context),
                NullLogger<GamesService>.Instance);
        }

        #region GetGameByAdventureIdAndUserId

        [Fact]
        public async void GetGameByAdventureIdAndUserId_Exists_ReturnGame()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            context.Games.Add(testGame);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act 
            var game = await service.GetGameByAdventureIdAndUserId(testGame.AdventureId, testGame.UserId);
            
            // assert
            Assert.NotNull(game);
            Assert.Equal(testGame.Id, game.Id);
        }
        
        [Fact]
        public async void GetGameByAdventureIdAndUserId_WrongAdventureId_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            context.Games.Add(testGame);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act 
            var game = await service.GetGameByAdventureIdAndUserId(Guid.NewGuid(), testGame.UserId);
            
            // assert
            Assert.Null(game);
        }
        
        [Fact]
        public async void GetGameByAdventureIdAndUserId_WrongUserId_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            context.Games.Add(testGame);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act 
            var game = await service.GetGameByAdventureIdAndUserId(testGame.AdventureId, Guid.NewGuid());
            
            // assert
            Assert.Null(game);
        }

        #endregion

        #region AddGame

        [Fact]
        public async void AddGame_GameAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            var service = CreateService(context);
            
            // act 
            service.AddGame(testGame);
            service.SaveChanges();
            
            // assert
            Assert.Single(context.Games);
            Assert.Equal(testGame.Id, context.Games.First().Id);
        }

        #endregion
    }
}