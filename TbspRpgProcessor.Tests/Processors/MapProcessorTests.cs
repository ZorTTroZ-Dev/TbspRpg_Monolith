using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
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
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                new List<Content>());
            
            // act
            Task Act() => processor.ChangeLocationViaRoute(Guid.NewGuid(),
                testRoute.Id, DateTime.UtcNow);

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
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                new List<Content>());
            
            // act
            Task Act() => processor.ChangeLocationViaRoute(
                testGames[0].Id,
                Guid.NewGuid(), 
                DateTime.UtcNow);

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
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                new List<Content>());
            
            // act
            Task Act() => processor.ChangeLocationViaRoute(
                testGames[0].Id,
                testRoute.Id,
                DateTime.UtcNow);

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
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                testContents,
                null,
                testSources);
            
            // act
            await processor.ChangeLocationViaRoute(
                testGames[0].Id,
                testRoute.Id,
                DateTime.UtcNow);
            
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
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                testContents,
                new List<Script>() { testScript },
                testSources);
            
            // act
            await processor.ChangeLocationViaRoute(
                testGames[0].Id,
                testRoute.Id,
                DateTime.UtcNow);
            
            // assert
            var game = testGames[0];
            Assert.Equal(testRoute.DestinationLocationId, game.LocationId);
            Assert.True(game.LocationUpdateTimeStamp > 0);
            Assert.Equal(2, testContents.Count);
            Assert.Equal(testContents[0].SourceKey, testRoute.RouteTakenSourceKey);
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
		                result = true
	                end
                ",
                Type = ScriptTypes.LuaScript
            };
            var resultSourceKey = Guid.NewGuid();
            var testScriptReturnKey = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "destination key script",
                Content = $"function run() result = '{resultSourceKey}' end",
                Type = ScriptTypes.LuaScript
            };
            
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
                    Key = testDestinationLocation.SourceKey,
                    ScriptId = testScriptReturnKey.Id,
                    Script = testScriptReturnKey
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
            var processor = CreateMapProcessor(
                testGames, 
                new List<Route>() { testRoute },
                testContents,
                new List<Script>() { testScript, testScriptReturnKey },
                testSources);
            
            // act
            await processor.ChangeLocationViaRoute(
                testGames[0].Id,
                testRoute.Id,
                DateTime.UtcNow);
            
            // assert
            var game = testGames[0];
            Assert.Equal(testRoute.DestinationLocationId, game.LocationId);
            Assert.True(game.LocationUpdateTimeStamp > 0);
            Assert.Equal(2, testContents.Count);
            Assert.Equal(testRoute.RouteTakenSourceKey, testContents[0].SourceKey);
            Assert.Equal(resultSourceKey, testContents[1].SourceKey);
        }

        #endregion
    }
}