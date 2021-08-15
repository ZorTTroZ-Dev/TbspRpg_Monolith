using System;
using System.Linq;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Repositories;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories
{
    public class GameRepositoryTests : InMemoryTest
    {
        public GameRepositoryTests() : base("GameRepositoryTests") {}

        #region GetGameById

        [Fact]
        public async void GetGameById_Valid_ReturnGame()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            context.Games.Add(testGame);
            await context.SaveChangesAsync();
            var gameRepository = new GameRepository(context);
            
            // act
            var game = await gameRepository.GetGameById(testGame.Id);
            
            // assert
            Assert.NotNull(game);
            Assert.Equal(testGame.Id, game.Id);
        }

        [Fact]
        public async void GetGameById_Invalid_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            context.Games.Add(testGame);
            await context.SaveChangesAsync();
            var gameRepository = new GameRepository(context);
            
            // act
            var game = await gameRepository.GetGameById(Guid.NewGuid());
            
            // assert
            Assert.Null(game);
        }

        #endregion

        #region GetGameByAdventureIdAndUserId

        [Fact]
        public async void GetGameByAdventureIdAndUserId_Valid_ReturnGame()
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
            var gameRepository = new GameRepository(context);
            
            // act
            var game = await gameRepository.GetGameByAdventureIdAndUserId(
                testGame.AdventureId, testGame.UserId);
            
            // assert
            Assert.NotNull(game);
            Assert.Equal(testGame.Id, game.Id);
        }
        
        [Fact]
        public async void GetGameByAdventureIdAndUserId_InValidAdventureId_ReturnNull()
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
            var gameRepository = new GameRepository(context);
            
            // act
            var game = await gameRepository.GetGameByAdventureIdAndUserId(
                Guid.NewGuid(), testGame.UserId);
            
            // assert
            Assert.Null(game);
        }
        
        [Fact]
        public async void GetGameByAdventureIdAndUserId_InValidUserId_ReturnNull()
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
            var gameRepository = new GameRepository(context);
            
            // game
            var game = await gameRepository.GetGameByAdventureIdAndUserId(
                testGame.AdventureId, Guid.NewGuid());
            
            // assert
            Assert.Null(game);
        }

        #endregion

        #region AddGame

        [Fact]
        public async void AddGame_Valid_GameAdded()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var game = new Game()
            {
                Id = Guid.NewGuid()
            };
            var gameRepository = new GameRepository(context);

            // act
            await gameRepository.AddGame(game);
            await context.SaveChangesAsync();
            
            // assert
            Assert.Single(context.Games);
            Assert.NotNull(context.Games.AsQueryable().FirstOrDefault());
            // ReSharper disable once PossibleNullReferenceException
            Assert.Equal(game.Id, context.Games.AsQueryable().First().Id);
        }

        #endregion

        #region GetGameByIdIncludeLocation

        [Fact]
        public async void GetGameByIdIncludeLocation()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testLocationId = Guid.NewGuid();
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocationId,
                Location = new Location()
                {
                    Id = testLocationId,
                    Name = "test location",
                    Initial = true
                }
            };
            context.Games.Add(testGame);
            await context.SaveChangesAsync();
            var repository = new GameRepository(context);
            
            // act
            var game = await repository.GetGameByIdWithLocation(testGame.Id);
            
            // assert
            Assert.NotNull(game);
            Assert.NotNull(game.Location);
            Assert.Equal(testLocationId, game.Location.Id);
        }

        #endregion
    }
}