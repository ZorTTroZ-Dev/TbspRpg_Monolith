using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class GameProcessorTests : ProcessorTest
    {
        #region StartGame

        [Fact]
        public async void StartGame_InvalidUserId_ThrowsException()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures);
            
            // act
            Task Act() => processor.StartGame(Guid.NewGuid(),
                testAdventures[0].Id, DateTime.Now);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void StartGame_InvalidAdventureId_ThrowsException()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures);
            
            // act
            Task Act() => processor.StartGame(testUsers[0].Id,
                Guid.NewGuid(), DateTime.Now);

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void StartGame_GameExists_ReturnsGame()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    UserId = testUsers[0].Id
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                testGames);

            // act
            var game = await processor.StartGame(testUsers[0].Id,
                testAdventures[0].Id, DateTime.Now);

            // assert
            Assert.Single(testGames);
            Assert.NotNull(game);
            Assert.Equal(testAdventures[0].Id, game.AdventureId);
            Assert.Equal(testUsers[0].Id, game.UserId);
        }
        
        [Fact]
        public async void StartGame_LocationDoesntExist_ThrowsException()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                new List<Game>(),
                new List<Location>());
            
            // act
            Task Act() => processor.StartGame(testUsers[0].Id,
                testAdventures[0].Id, DateTime.Now);

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }
        
        [Fact]
        public async void StartGame_ValidWithInitScript_GameCreated()
        {
            // arrange
            var testScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "test script",
                Content = @"
                    function run()
		                result = true
	                end
                ",
                Type = ScriptTypes.LuaScript
            };
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    InitialSourceKey = Guid.NewGuid(),
                    InitializationScriptId = testScript.Id
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var testLocations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    Initial = true,
                    SourceKey = Guid.NewGuid()
                }
            };
            var testContents = new List<Content>();
            var testGames = new List<Game>();
            var testScripts = new List<Script>() { testScript };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                testGames,
                testLocations,
                testContents,
                testScripts);
            
            // act
            var game = await processor.StartGame(testUsers[0].Id,
                testAdventures[0].Id, DateTime.UtcNow);

            // assert
            Assert.Single(testGames);
            Assert.NotNull(game);
            Assert.True(game.LocationUpdateTimeStamp > 0);
            Assert.Equal(testAdventures[0].Id, game.AdventureId);
            Assert.Equal(testUsers[0].Id, game.UserId);
            Assert.Equal(testLocations[0].Id, game.LocationId);
            Assert.Equal(2, testContents.Count);
            Assert.NotNull(testContents.FirstOrDefault(c => c.SourceKey == testAdventures[0].InitialSourceKey));
            Assert.NotNull(testContents.FirstOrDefault(c => c.SourceKey == testLocations[0].SourceKey));
        }
        
        [Fact]
        public async void StartGame_ValidWithoutInitScript_GameCreated()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure",
                    InitialSourceKey = Guid.NewGuid(),
                    InitializationScriptId = null
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var testLocations = new List<Location>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    Initial = true,
                    SourceKey = Guid.NewGuid()
                }
            };
            var testContents = new List<Content>();
            var testGames = new List<Game>();
            var testScripts = new List<Script>();
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                testGames,
                testLocations,
                testContents,
                testScripts);
            
            // act
            var game = await processor.StartGame(testUsers[0].Id,
                testAdventures[0].Id, DateTime.UtcNow);

            // assert
            Assert.Single(testGames);
            Assert.NotNull(game);
            Assert.True(game.LocationUpdateTimeStamp > 0);
            Assert.Equal(testAdventures[0].Id, game.AdventureId);
            Assert.Equal(testUsers[0].Id, game.UserId);
            Assert.Equal(testLocations[0].Id, game.LocationId);
            Assert.Equal(2, testContents.Count);
            Assert.NotNull(testContents.FirstOrDefault(c => c.SourceKey == testAdventures[0].InitialSourceKey));
            Assert.NotNull(testContents.FirstOrDefault(c => c.SourceKey == testLocations[0].SourceKey));
        }

        #endregion

        #region RemoveGame

        [Fact]
        public async void RemoveGame_InvalidGameId_ExceptionThrown()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    UserId = testUsers[0].Id
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                testGames);
            
            // act
            Task Act() => processor.RemoveGame(new GameRemoveModel()
            {
                GameId = Guid.NewGuid()
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void RemoveGame_NoContent_GameRemoved()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    UserId = testUsers[0].Id
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                testGames,
                null,
                new List<Content>());
            
            // act
            await processor.RemoveGame(new GameRemoveModel()
            {
                GameId = testGames[0].Id
            });
            
            // assert
            Assert.Empty(testGames);
        }

        [Fact]
        public async void RemoveGame_Valid_GameAndContentRemoved()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    UserId = testUsers[0].Id
                }
            };
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[0].Id,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[0].Id,
                    Position = 1,
                    SourceKey = Guid.NewGuid()
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                testGames,
                null,
                testContents);
            
            // act
            await processor.RemoveGame(new GameRemoveModel()
            {
                GameId = testGames[0].Id
            });
            
            // assert
            Assert.Empty(testGames);
            Assert.Empty(testContents);
        }

        #endregion

        #region RemoveGames

        [Fact]
        public async void RemoveGames_AllGamesRemoved()
        {
            // arrange
            var testAdventures = new List<Adventure>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "test adventure"
                }
            };
            var testUsers = new List<User>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Email = "test"
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    UserId = testUsers[0].Id
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = testAdventures[0].Id,
                    UserId = testUsers[0].Id
                }
            };
            var testContents = new List<Content>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[0].Id,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[0].Id,
                    Position = 1,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[1].Id,
                    Position = 0,
                    SourceKey = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    GameId = testGames[1].Id,
                    Position = 1,
                    SourceKey = Guid.NewGuid()
                }
            };
            var processor = CreateGameProcessor(
                testUsers,
                testAdventures,
                testGames,
                null,
                testContents);
            
            // act
            await processor.RemoveGames(new List<Game>()
            {
                testGames[0], testGames[1]
            });
            
            // assert
            Assert.Empty(testGames);
            Assert.Empty(testContents);
        }

        #endregion
    }
}