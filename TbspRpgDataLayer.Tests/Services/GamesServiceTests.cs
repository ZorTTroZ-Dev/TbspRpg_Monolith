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
            await service.AddGame(testGame);
            await service.SaveChanges();
            
            // assert
            Assert.Single(context.Games);
            Assert.Equal(testGame.Id, context.Games.First().Id);
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
            var service = CreateService(context);
            
            // act
            var game = await service.GetGameByIdIncludeLocation(testGame.Id);
            
            // assert
            Assert.NotNull(game);
            Assert.NotNull(game.Location);
            Assert.Equal(testLocationId, game.Location.Id);
        }

        #endregion

        #region GetGameById

        [Fact]
        public async void GetGameById_GameExists_ReturnGame()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            context.Games.Add(testGame);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var game = await service.GetGameById(testGame.Id);
            
            // assert
            Assert.NotNull(game);
            Assert.Equal(testGame.Id, game.Id);
        }
        
        [Fact]
        public async void GetGameById_NotExists_ReturnNull()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                Id = Guid.NewGuid()
            };
            context.Games.Add(testGame);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var game = await service.GetGameById(Guid.NewGuid());
            
            // assert
            Assert.Null(game);
        }

        #endregion

        #region GetGames

        [Fact]
        public async void GetGames_NoFilters_ReturnsAll()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                AdventureId = Guid.NewGuid()
            };
            var testGameTwo = new Game()
            {
                AdventureId = Guid.NewGuid()
            };
            await context.Games.AddAsync(testGame);
            await context.Games.AddAsync(testGameTwo);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var games = await service.GetGames(null);
            
            // assert
            Assert.Equal(2, games.Count);
        }

        #endregion

        #region GetGamesByAdventureId

        [Fact]
        public async void GetGamesByAdventureId_ValidAdventureId_ReturnsGames()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                AdventureId = Guid.NewGuid()
            };
            var testGameTwo = new Game()
            {
                AdventureId = Guid.NewGuid()
            };
            await context.Games.AddAsync(testGame);
            await context.Games.AddAsync(testGameTwo);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            var games = await service.GetGamesByAdventureId(testGame.AdventureId);
            
            // assert
            Assert.Single(games);
            Assert.Equal(testGame.AdventureId, games[0].AdventureId);
        }

        #endregion

        #region RemoveGame

        [Fact]
        public async void RemoveGame_GameRemoved()
        {
            // arrange
            await using var context = new DatabaseContext(DbContextOptions);
            var testGame = new Game()
            {
                AdventureId = Guid.NewGuid()
            };
            var testGameTwo = new Game()
            {
                AdventureId = Guid.NewGuid()
            };
            await context.Games.AddAsync(testGame);
            await context.Games.AddAsync(testGameTwo);
            await context.SaveChangesAsync();
            var service = CreateService(context);
            
            // act
            service.RemoveGame(testGame);
            await service.SaveChanges();

            // assert
            Assert.Single(context.Games);
        }

        #endregion
    }
}