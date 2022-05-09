using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.Entities;
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
            var processor = CreateSourceProcessor(new List<En>() {testEn});
            
            // act
            Task Act() => processor.CreateOrUpdateSource(new Source()
            {
                Id = testEn.Id,
                AdventureId = testEn.AdventureId,
                Key = Guid.NewGuid()
            }, Languages.ENGLISH);

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
            var processor = CreateSourceProcessor(sources);
            
            
            // act
            var dbSource = await processor.CreateOrUpdateSource(new Source()
            {
                AdventureId = testEn.AdventureId,
                Key = Guid.Empty,
                Text = "new source"
            }, Languages.ENGLISH);
            
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
            var processor = CreateSourceProcessor(new List<En>() {testEn});
            
            // act
            var dbSource = await processor.CreateOrUpdateSource(new Source()
            {
                Id = testEn.Id,
                Key = testEn.Key,
                AdventureId = testEn.AdventureId,
                Text = "updated text"
            }, Languages.ENGLISH);
            
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
            var processor = CreateSourceProcessor(sources);
            
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
            var processor = CreateSourceProcessor(sources);
            
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
            var processor = CreateSourceProcessor(sources);
            
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

        #region ResolveSourceKey

        [Fact]
        public async void ResolveSourceKey_NoScript_ReturnKey()
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
            var processor = CreateSourceProcessor(sources);
            
            // act
            var key = await processor.ResolveSourceKey(new SourceForKeyModel()
            {
                Key = testEn.Key,
                AdventureId = testEn.AdventureId,
                Language = Languages.ENGLISH
            });

            // assert
            Assert.Equal(testEn.Key, key);
        }

        [Fact]
        public async void ResolveSourceKey_OneScript_ReturnKey()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "This is a text with some *emphasis*"
            };
            var testScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{testEn.Key}' end"
            };
            var scriptEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScript.Id,
                Script = testScript
            };
            var sources = new List<En>()
            {
                testEn, scriptEn
            };
            var scripts = new List<Script>()
            {
                testScript
            };
            var processor = CreateSourceProcessor(sources, scripts);
            
            // act
            var key = await processor.ResolveSourceKey(new SourceForKeyModel()
            {
                Key = scriptEn.Key,
                AdventureId = scriptEn.AdventureId,
                Language = Languages.ENGLISH
            });

            // assert
            Assert.Equal(testEn.Key, key);
        }

        [Fact]
        public async void ResolveSourceKey_MultipleScript_ReturnKey()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "This is a text with some *emphasis*"
            };
            
            // source one with script
            var testScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{testEn.Key}' end"
            };
            var scriptEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScript.Id,
                Script = testScript
            };
            
            // source two with script
            var testScriptTwo = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script two",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{scriptEn.Key}' end"
            };
            var scriptEnTwo = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScriptTwo.Id,
                Script = testScriptTwo
            };
            
            var sources = new List<En>()
            {
                testEn, scriptEn, scriptEnTwo
            };
            var scripts = new List<Script>()
            {
                testScript, testScriptTwo
            };
            var processor = CreateSourceProcessor(sources, scripts);
            
            // act
            var key = await processor.ResolveSourceKey(new SourceForKeyModel()
            {
                Key = scriptEnTwo.Key,
                AdventureId = scriptEn.AdventureId,
                Language = Languages.ENGLISH
            });

            // assert
            Assert.Equal(testEn.Key, key);
        }

        [Fact]
        public async void ResolveSourceKey_InvalidKey_ExceptionThrown()
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
            var processor = CreateSourceProcessor(sources);
            
            // act
            Task Act() => processor.ResolveSourceKey(new SourceForKeyModel()
            {
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                Language = Languages.ENGLISH
            });

            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }

        [Fact]
        public async void ResloveSourceKey_ScriptReturnNonGuid_ExceptionThrown()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "This is a text with some *emphasis*"
            };
            var testScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = 'Fred' end"
            };
            var scriptEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScript.Id,
                Script = testScript
            };
            var sources = new List<En>()
            {
                testEn, scriptEn
            };
            var scripts = new List<Script>()
            {
                testScript
            };
            var processor = CreateSourceProcessor(sources, scripts);
            
            // act
            Task Act() => processor.ResolveSourceKey(new SourceForKeyModel()
            {
                Key = scriptEn.Key,
                AdventureId = scriptEn.AdventureId,
                Language = Languages.ENGLISH
            });
            
            // assert
            await Assert.ThrowsAsync<FormatException>(Act);
        }
        
        [Fact]
        public async void ResloveSourceKey_ScriptReturnInvalidKey_ExceptionThrown()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "This is a text with some *emphasis*"
            };
            var testScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{Guid.NewGuid()}' end"
            };
            var scriptEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScript.Id,
                Script = testScript
            };
            var sources = new List<En>()
            {
                testEn, scriptEn
            };
            var scripts = new List<Script>()
            {
                testScript
            };
            var processor = CreateSourceProcessor(sources, scripts);
            
            // act
            Task Act() => processor.ResolveSourceKey(new SourceForKeyModel()
            {
                Key = scriptEn.Key,
                AdventureId = scriptEn.AdventureId,
                Language = Languages.ENGLISH
            });
            
            // assert
            await Assert.ThrowsAsync<ArgumentException>(Act);
        }
        
        [Fact]
        public async void ResolveSourceKey_MoreThanFiveScripts_ExceptionThrown()
        {
            // arrange
            var testEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Text = "This is a text with some *emphasis*"
            };
            
            // source one with script
            var testScript = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{testEn.Key}' end"
            };
            var scriptEn = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScript.Id,
                Script = testScript
            };
            
            // source two with script
            var testScriptTwo = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script two",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{scriptEn.Key}' end"
            };
            var scriptEnTwo = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScriptTwo.Id,
                Script = testScriptTwo
            };
            
            // source three with script
            var testScriptThree = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script three",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{scriptEnTwo.Key}' end"
            };
            var scriptEnThree = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScriptThree.Id,
                Script = testScriptThree
            };
            
            // source four with script
            var testScriptFour = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script four",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{scriptEnThree.Key}' end"
            };
            var scriptEnFour = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScriptFour.Id,
                Script = testScriptFour
            };
            
            // source five with script
            var testScriptFive = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script five",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{scriptEnFour.Key}' end"
            };
            var scriptEnFive = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScriptFive.Id,
                Script = testScriptFive
            };
            
            // source six with script
            var testScriptSix = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "source resolve script six",
                Type = ScriptTypes.LuaScript,
                Content = $"function run() result = '{scriptEnFive.Key}' end"
            };
            var scriptEnSix = new En()
            {
                Id = Guid.NewGuid(),
                Key = Guid.NewGuid(),
                AdventureId = testEn.AdventureId,
                ScriptId = testScriptSix.Id,
                Script = testScriptSix
            };
            
            var sources = new List<En>()
            {
                testEn, scriptEn, scriptEnTwo, scriptEnThree, scriptEnFour, scriptEnFive, scriptEnSix
            };
            var scripts = new List<Script>()
            {
                testScript, testScriptTwo, testScriptThree, testScriptFour, testScriptFive, testScriptSix
            };
            var processor = CreateSourceProcessor(sources, scripts);
            
            // act
            Task Act() => processor.ResolveSourceKey(new SourceForKeyModel()
            {
                Key = scriptEnSix.Key,
                AdventureId = scriptEn.AdventureId,
                Language = Languages.ENGLISH
            });

            // assert
            await Assert.ThrowsAsync<Exception>(Act);
        }

        #endregion
    }
}