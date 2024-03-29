﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class MapProcessorTests: ProcessorTest
    {
        #region ChangeLocationViaRoute

        [Fact]
        public async void ChangeLocationViaRoute_InvalidGameId_ThrowsExcpetion()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                RouteTakenSourceKey = Guid.NewGuid()
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = testRoute.LocationId
                }
            };
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                new List<Route>() {testRoute},
                null,
                null,
                testGames,
                new List<Content>());
            
            // act
            Task Act() => processor.ChangeLocationViaRoute(new MapChangeLocationModel() {
                GameId = Guid.NewGuid(),
                RouteId = testRoute.Id,
                TimeStamp = DateTime.UtcNow
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void ChangeLocationViaRoute_InvalidRouteId_ThrowsExcpetion()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                RouteTakenSourceKey = Guid.NewGuid()
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = testRoute.LocationId
                }
            };
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                new List<Route>() {testRoute},
                null,
                null,
                testGames,
                new List<Content>());
            
            // act
            Task Act() => processor.ChangeLocationViaRoute(new MapChangeLocationModel() {
                GameId = testGames[0].Id,
                RouteId = Guid.NewGuid(), 
                TimeStamp = DateTime.UtcNow
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void ChangeLocationViaRoute_GameWrongLocation_ThrowsExcpetion()
        {
            // arrange
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = Guid.NewGuid(),
                DestinationLocationId = Guid.NewGuid(),
                RouteTakenSourceKey = Guid.NewGuid()
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = Guid.NewGuid()
                }
            };
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                new List<Route>() {testRoute},
                null,
                null,
                testGames,
                new List<Content>());
            
            // act
            Task Act() => processor.ChangeLocationViaRoute(new MapChangeLocationModel() {
                GameId = testGames[0].Id,
                RouteId = testRoute.Id,
                TimeStamp = DateTime.UtcNow
            });

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }
        
        [Fact]
        public async void ChangeLocationViaRoute_Valid_LocationUpdated()
        {
            // arrange
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                SourceKey = Guid.NewGuid()
            };
            var testLocation = new Location()
            {
                Id = Guid.NewGuid()
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocation.Id,
                Location = testLocation,
                DestinationLocationId = testDestinationLocation.Id,
                DestinationLocation = testDestinationLocation,
                RouteTakenSourceKey = Guid.NewGuid()
            };
            var testSources = new List<En>()
            {
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testRoute.RouteTakenSourceKey
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testDestinationLocation.SourceKey
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = testRoute.LocationId
                }
            };
            var testContents = new List<Content>();
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                new List<Route>() {testRoute},
                null,
                testSources,
                testGames,
                testContents);
            
            // act
            await processor.ChangeLocationViaRoute(new MapChangeLocationModel() {
                GameId = testGames[0].Id,
                RouteId = testRoute.Id,
                TimeStamp = DateTime.UtcNow
            });
            
            // assert
            var game = testGames[0];
            Assert.Equal(testRoute.DestinationLocationId, game.LocationId);
            Assert.True(game.LocationUpdateTimeStamp > 0);
            Assert.Equal(2, testContents.Count);
            Assert.Equal(testContents[0].SourceKey, testRoute.RouteTakenSourceKey);
        }
        
        [Fact]
        public async void ChangeLocationViaRoute_ValidFinalLocationWithScripts_LocationUpdated()
        {
            var exitLocationTestScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "test script",
                Content = @"
                    function run()
                        game:SetGameStatePropertyBoolean('LocationExited', true)
		                result = true
	                end
                ",
                Type = ScriptTypes.LuaScript
            };
            var enterLocationTestScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "test script",
                Content = @"
                    function run()
                        routeTaken = game:GetGameStatePropertyNumber('RouteTaken')
                        game:SetGameStatePropertyNumber('LocationEntered', routeTaken + 1)
                        
		                result = true
	                end
                ",
                Type = ScriptTypes.LuaScript
            };
            var routeTakenTestScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "test script",
                Content = @"
                    function run()
                        game:SetGameStatePropertyNumber('RouteTaken', 42)
		                result = true
	                end
                ",
                Type = ScriptTypes.LuaScript
            };
            var terminationTestScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "test script",
                Content = @"
                    function run()
                        game:SetGameStatePropertyBoolean('GameTerminated', true)
		                result = true
	                end
                ",
                Type = ScriptTypes.LuaScript
            };
            // arrange
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                SourceKey = Guid.NewGuid(),
                Final = true,
                EnterScriptId = enterLocationTestScript.Id
            };
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                ExitScriptId = exitLocationTestScript.Id
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocation.Id,
                Location = testLocation,
                DestinationLocationId = testDestinationLocation.Id,
                DestinationLocation = testDestinationLocation,
                RouteTakenSourceKey = Guid.NewGuid(),
                RouteTakenScriptId = routeTakenTestScript.Id
            };
            var testSources = new List<En>()
            {
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testRoute.RouteTakenSourceKey
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testDestinationLocation.SourceKey
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = testRoute.LocationId,
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test",
                        TerminationScriptId = terminationTestScript.Id
                    }
                }
            };
            var testContents = new List<Content>();
            var processor = CreateTbspRpgProcessor(
                null,
                new List<Script>() { exitLocationTestScript, enterLocationTestScript, routeTakenTestScript, terminationTestScript },
                null,
                new List<Route>() {testRoute},
                null,
                testSources,
                testGames,
                testContents);
            
            // act
            await processor.ChangeLocationViaRoute(new MapChangeLocationModel() {
                GameId = testGames[0].Id,
                RouteId = testRoute.Id,
                TimeStamp = DateTime.UtcNow
            });
            
            // assert
            var game = testGames[0];
            Assert.Equal(testRoute.DestinationLocationId, game.LocationId);
            Assert.True(game.LocationUpdateTimeStamp > 0);
            Assert.Equal(2, testContents.Count);
            Assert.Equal(testContents[0].SourceKey, testRoute.RouteTakenSourceKey);
            Assert.NotNull(game.GameState);
            Assert.Equal("{\"LocationExited\":true,\"RouteTaken\":42,\"LocationEntered\":43,\"GameTerminated\":true}", game.GameState);
        }
        
        [Fact]
        public async void ChangeLocationViaRoute_ResolveDestinationSourceKey_LocationUpdated()
        {
            var testScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "test script",
                Content = @"
                    function run()
                        game:SetGameStatePropertyBoolean('ScriptRun', true)
		                result = true
	                end
                ",
                Type = ScriptTypes.LuaScript
            };
            var resultSourceKey = Guid.NewGuid();
            var badResultSourceKey = Guid.NewGuid();
            
            // arrange
            var testDestinationLocation = new Location()
            {
                Id = Guid.NewGuid(),
                SourceKey = Guid.NewGuid(),
                Final = true,
                EnterScriptId = testScript.Id
            };
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                ExitScriptId = testScript.Id
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                LocationId = testLocation.Id,
                Location = testLocation,
                DestinationLocationId = testDestinationLocation.Id,
                DestinationLocation = testDestinationLocation,
                RouteTakenSourceKey = Guid.NewGuid(),
                RouteTakenScriptId = testScript.Id
            };
            var testSources = new List<En>()
            {
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testRoute.RouteTakenSourceKey
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = resultSourceKey
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = badResultSourceKey
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testDestinationLocation.SourceKey
                }
            };
            var testGames = new List<Game>()
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    LocationId = testRoute.LocationId,
                    Adventure = new Adventure()
                    {
                        Id = Guid.NewGuid(),
                        Name = "test",
                        TerminationScriptId = testScript.Id
                    }
                }
            };
            var testContents = new List<Content>();
            var processor = CreateTbspRpgProcessor(
                null,
                new List<Script>() { testScript },
                null,
                new List<Route>() {testRoute},
                null,
                testSources,
                testGames,
                testContents);
            
            // act
            await processor.ChangeLocationViaRoute(new MapChangeLocationModel() {
                GameId = testGames[0].Id,
                RouteId = testRoute.Id,
                TimeStamp = DateTime.UtcNow
            });
            
            // assert
            var game = testGames[0];
            Assert.Equal(testRoute.DestinationLocationId, game.LocationId);
            Assert.True(game.LocationUpdateTimeStamp > 0);
            Assert.Equal(2, testContents.Count);
            Assert.Equal(testRoute.RouteTakenSourceKey, testContents[0].SourceKey);
            Assert.Equal(testDestinationLocation.SourceKey, testContents[1].SourceKey);
        }

        #endregion
    }
}