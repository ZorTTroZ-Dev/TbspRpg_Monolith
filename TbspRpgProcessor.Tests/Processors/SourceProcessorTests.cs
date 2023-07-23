using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors
{
    public class SourceProcessorTests : ProcessorTest
    {
        #region CreateOrUpdateSource

        [Fact]
        public async void CreateOrUpdateSource_BadSourceId_ThrowException()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var processor = CreateTbspRpgProcessor(null, null, null, null, null,
                new List<En>() {testEn});
            
            // act
            Task Act() => processor.CreateOrUpdateSource(new SourceCreateOrUpdateModel() {
                Source = new Source()
                {
                    Id = testEn.Id,
                    AdventureId = testEn.AdventureId,
                    Key = Guid.NewGuid()
                },
                Language = Languages.ENGLISH
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void CreateOrUpdateSource_EmptyKey_CreateNewSource()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid()
            };
            var sources = new List<En>()
            {
                testEn
            };
            var processor = CreateTbspRpgProcessor(null, null, null, null, null,
                sources);
            
            
            // act
            var dbSource = await processor.CreateOrUpdateSource(new SourceCreateOrUpdateModel() {
                Source = new Source()
                {
                    AdventureId = testEn.AdventureId,
                    Key = Guid.Empty,
                    Text = "new source"
                }, 
                Language = Languages.ENGLISH
            });
            
            // assert
            Assert.NotNull(dbSource);
            Assert.Equal(2, sources.Count);
            Assert.Equal("new source", dbSource.Text);
            Assert.NotEqual(Guid.Empty, dbSource.Key);
        }

        [Fact]
        public async void CreateOrUpdateSource_ExistingSource_UpdateKey()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "original text"
            };
            var sources = new List<En>()
            {
                testEn
            };
            var processor = CreateTbspRpgProcessor(null, null, null, null, null,
                new List<En>() {testEn});
            
            // act
            var dbSource = await processor.CreateOrUpdateSource(new SourceCreateOrUpdateModel() {
                Source = new Source()
                {
                    Id = testEn.Id,
                    Key = testEn.Key,
                    AdventureId = testEn.AdventureId,
                    Text = "updated text"
                },
                Language = Languages.ENGLISH
            });
            
            // assert
            Assert.Single(sources);
            Assert.Equal("updated text", dbSource.Text);
        }

        #endregion

        #region GetSourceForKey

        [Fact]
        public async void GetSourceForKey_DoesntExist_ReturnNull()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "original text"
            };
            var sources = new List<En>()
            {
                testEn
            };
            var processor = CreateTbspRpgProcessor(null, null, null, null, null,
                sources);
            
            // act
            var source = await processor.GetSourceForKey(new SourceForKeyModel()
            {
                Key = testEn.Key,
                AdventureId = Guid.NewGuid(),
                Language = Languages.ENGLISH
            });
            
            // assert
            Assert.Null(source);
        }

        [Fact]
        public async void GetSourceForKey_Valid_SourceTextIsHtml()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "This is a text with some *emphasis*"
            };
            var sources = new List<En>()
            {
                testEn
            };
            var processor = CreateTbspRpgProcessor(null, null, null, null, null,
                sources);
            
            // act
            var source = await processor.GetSourceForKey(new SourceForKeyModel()
            {
                Key = testEn.Key,
                AdventureId = testEn.AdventureId,
                Language = Languages.ENGLISH,
                Processed = true
            });
            
            // Assert
            Assert.Equal("<p>This is a text with some <em>emphasis</em></p>", source.Text);
        }
        
        [Fact]
        public async void GetSourceForKey_ValidNotProcessed_SourceTextNotHtml()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "This is a text with some *emphasis*"
            };
            var sources = new List<En>()
            {
                testEn
            };
            var processor = CreateTbspRpgProcessor(null, null, null, null, null,
                sources);
            
            // act
            var source = await processor.GetSourceForKey(new SourceForKeyModel()
            {
                Key = testEn.Key,
                AdventureId = testEn.AdventureId,
                Language = Languages.ENGLISH,
                Processed = false
            });
            
            // Assert
            Assert.Equal("This is a text with some *emphasis*", source.Text);
        }

        #endregion

        #region GetUnreferencedSource

        [Fact]
        public async void GetUnreferencedSource_OneNotUsed_SourceReturned()
        {
            // arrange
            var testAdventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                InitialSourceKey = Guid.NewGuid()
            };
            var testLocation = new Location()
            {
                Id = Guid.NewGuid(),
                Adventure = testAdventure,
                AdventureId = testAdventure.Id,
                SourceKey = Guid.NewGuid()
            };
            var testRoute = new Route()
            {
                Id = Guid.NewGuid(),
                Location = testLocation,
                LocationId = testLocation.Id,
                SourceKey = Guid.NewGuid()
            };
            var testGame = new Game()
            {
                Id = Guid.NewGuid(),
                Adventure = testAdventure,
                AdventureId = testAdventure.Id
            };
            var testContent = new Content()
            {
                Game = testGame,
                GameId = testGame.Id,
                Id = Guid.NewGuid(),
                SourceKey = Guid.NewGuid()
            };
            var testScript = new Script()
            {
                Id = Guid.NewGuid(),
                AdventureId = testAdventure.Id,
                Adventure = testAdventure,
                Content = Guid.NewGuid().ToString()
            };
            var testSources = new List<En>()
            {
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testAdventure.InitialSourceKey,
                    AdventureId = testAdventure.Id
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testLocation.SourceKey,
                    AdventureId = testAdventure.Id
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testRoute.SourceKey,
                    AdventureId = testAdventure.Id
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = testContent.SourceKey,
                    AdventureId = testAdventure.Id
                },
                new En()
                {
                    Id = Guid.NewGuid(),
                    Key = Guid.Parse(testScript.Content),
                    AdventureId = testAdventure.Id
                },
                new En() // not referenced or used
                {
                    Id = Guid.NewGuid(),
                    Key = Guid.NewGuid(),
                    AdventureId = testAdventure.Id,
                    Text = "unreferenced"
                }
            };
            var testAdventures = new List<Adventure>() {testAdventure};
            var testLocations = new List<Location>() {testLocation};
            var testRoutes = new List<Route>() {testRoute};
            var testGames = new List<Game>() {testGame};
            var testContents = new List<Content>() {testContent};
            var testScripts = new List<Script>() {testScript};
            var processor = CreateTbspRpgProcessor(
                null,
                testScripts,
                testAdventures,
                testRoutes,
                testLocations,
                testSources,
                testGames,
                testContents);
            
            // act
            var unreferencedSources = await processor.GetUnreferencedSources(new UnreferencedSourceModel()
            {
                AdventureId = testAdventure.Id
            });
            
            // assert
            Assert.Single(unreferencedSources);
            Assert.Equal("unreferenced", unreferencedSources[0].Text);
        }

        #endregion
        
        #region RemoveSource

        [Fact]
        public async void RemoveSource_ValidSourceId_SourceRemoved()
        {
            // arrange
            var testSources = new List<En>()
            {
                new En()
                {
                    Id = Guid.NewGuid(),
                    Language = Languages.ENGLISH
                }
            };
            var processor = CreateTbspRpgProcessor(
                null,
                null,
                null,
                null,
                null,
                testSources);
            
            // act
            await processor.RemoveSource(new SourceRemoveModel()
            {
                SourceId = testSources[0].Id
            });
            
            // assert
            Assert.Empty(testSources);
        }

        #endregion
    }
}