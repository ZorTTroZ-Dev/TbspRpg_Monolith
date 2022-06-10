using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLua.Exceptions;
using TbspRpgApi.Entities.LanguageSources;
using TbspRpgDataLayer.Entities;
using TbspRpgProcessor.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgProcessor.Tests.Processors;

public class ScriptProcessorTests: ProcessorTest
{
    #region ExecuteScript

    [Fact]
    public async void ExecuteScript_NoIncludes_ScriptExecuted()
    {
        // arrange
        var script = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script",
            Content = @"
                function banana()
                    return 'foo'
                end

                function run()
		            result = banana()
	            end
            ",
            Type = ScriptTypes.LuaScript
        };
        var processor = CreateScriptProcessor(new List<Script>() {script});
        
        // act
        var result = await processor.ExecuteScript(script.Id);
        
        // assert
        Assert.Equal("foo", result);
    }

    [Fact]
    public async void ExecuteScript_WithIncludes_ScriptExecuted()
    {
        // arrange
        var script = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script",
            Content = @"
                function run()
		            result = banana()
	            end
            ",
            Type = ScriptTypes.LuaScript,
            Includes = new List<Script>()
            {
                new Script()
                {
                    Id = Guid.NewGuid(),
                    Name = "base script",
                    Content = @"
                        function banana()
                            return 'foo'
                        end
                    ",
                    Type = ScriptTypes.LuaScript
                }
            }
        };
        var processor = CreateScriptProcessor(new List<Script>() {script});
        
        // act
        var result = await processor.ExecuteScript(script.Id);
        
        // assert
        Assert.Equal("foo", result);
    }
    
    [Fact]
    public async void ExecuteScript_NoRunFunction_ExceptionThrown()
    {
        // arrange
        var script = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script",
            Content = @"
                function banana()
                    return 'foo'
                end

                function fun()
		            result = banana()
	            end
            ",
            Type = ScriptTypes.LuaScript
        };
        var processor = CreateScriptProcessor(new List<Script>() {script});
        
        // act
        Task Act() => processor.ExecuteScript(script.Id);
        
        // assert
        await Assert.ThrowsAsync<LuaScriptException>(Act);
    }
    
    [Fact]
    public async void ExecuteScript_BadScriptId_ExceptionThrown()
    {
        // arrange
        var script = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script",
            Content = @"
                function banana()
                    return 'foo'
                end

                function fun()
		            result = banana()
	            end
            ",
            Type = ScriptTypes.LuaScript
        };
        var processor = CreateScriptProcessor(new List<Script>() {script});
        
        // act
        Task Act() => processor.ExecuteScript(Guid.NewGuid());
        
        // assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }
    
    [Fact]
    public async void ExecuteScript_EmptyScriptId_ExceptionThrown()
    {
        // arrange
        var script = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script",
            Content = @"
                function banana()
                    return 'foo'
                end

                function fun()
		            result = banana()
	            end
            ",
            Type = ScriptTypes.LuaScript
        };
        var processor = CreateScriptProcessor(new List<Script>() {script});
        
        // act
        Task Act() => processor.ExecuteScript(Guid.Empty);
        
        // assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }

    #endregion
    
    #region UpdateScript

    [Fact]
    public async void UpdateScript_BadScriptId_ThrowException()
    {
        // arrange
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script"
        };
        var scripts = new List<Script>() { testScript };
        var processor = CreateScriptProcessor(scripts);
        
        // act
        Task Act() => processor.UpdateScript(new ScriptUpdateModel()
        {
            script = new Script()
            {
                Id = Guid.NewGuid(),
                Name = "banana"
            }
        });
    
        // assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }
    
    [Fact]
    public async void UpdateScript_EmptyScriptId_CreateNewScript()
    {
        // arrange
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test location",
            Type = ScriptTypes.LuaScript,
            Content = "function base()\n  result = 'banana'\nend",
            AdventureId = Guid.NewGuid()
        };
        var scripts = new List<Script>() { testScript };
        var processor = CreateScriptProcessor(scripts);
        
        // act
        await processor.UpdateScript(new ScriptUpdateModel()
        {
            script = new Script()
            {
                Id = Guid.Empty,
                Name = "new script",
                Type = ScriptTypes.LuaScript,
                Content = "function run()\n  base()\n  result = 'banana'\nend",
                AdventureId = testScript.AdventureId,
                Includes = new List<Script>()
                {
                    testScript
                }
            }
        });
        
        // assert
        Assert.Equal(2, scripts.Count);
        Assert.NotEqual(Guid.Empty, scripts[1].Id);
        Assert.Single(scripts[1].Includes);
    }
    
    [Fact]
    public async void UpdateLocation_ScriptChange_ScriptUpdated()
    {
        // arrange
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test location",
            Type = ScriptTypes.LuaScript,
            Content = "function base()\n  result = 'banana'\nend",
            AdventureId = Guid.NewGuid()
        };
        var testScriptTwo = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test location two",
            Type = ScriptTypes.LuaScript,
            Content = "function\n  result = 'banana'\nend",
            AdventureId = Guid.NewGuid()
        };
        var scripts = new List<Script>() { testScript, testScriptTwo };
        var processor = CreateScriptProcessor(scripts);
        
        // act
        await processor.UpdateScript(new ScriptUpdateModel()
        {
            script = new Script()
            {
                Id = testScriptTwo.Id,
                Name = "new script",
                Type = ScriptTypes.LuaScript,
                Content = "function run()\n  base()\n  result = 'banana'\nend",
                AdventureId = testScript.AdventureId,
                Includes = new List<Script>()
                {
                    testScript
                }
            }
        });
        
        // assert
        Assert.Equal(2, scripts.Count);
        Assert.NotEqual(Guid.Empty, scripts[1].Id);
        Assert.Single(scripts[1].Includes);
        Assert.Equal("new script", scripts[1].Name);
        Assert.Contains("base()", scripts[1].Content);
    }

    #endregion

    #region RemoveScript

    [Fact]
    public async void RemoveScript_InvalidId_ExceptionThrown()
    {
        // arrange
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script"
        };
        var processor = CreateScriptProcessor(
            new List<Script>() {testScript},
            new List<Adventure>(),
            new List<Route>(),
            new List<Location>(),
            new List<En>());
        
        // act
        Task Act() => processor.RemoveScript(new ScriptRemoveModel()
        {
            ScriptId = Guid.NewGuid()
        });
    
        // assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }
    
    [Fact]
    public async void RemoveScript_WithAdventures_ScriptRemoved()
    {
        // arrange
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script"
        };
        var testAdventure = new Adventure()
        {
            Id = Guid.NewGuid(),
            Name = "test adventure",
            InitializationScript = testScript
        };
        var processor = CreateScriptProcessor(
            new List<Script>() {testScript},
            new List<Adventure>() {testAdventure},
            new List<Route>(),
            new List<Location>(),
            new List<En>());
        
        // act
        await processor.RemoveScript(new ScriptRemoveModel()
        {
            ScriptId = testScript.Id
        });
    }

    [Fact]
    public async void RemoveScript_WithRoutes_ScriptRemoved()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async void RemoveScript_WithLocations_ScriptRemoved()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async void RemoveScript_WithSources_ScriptRemoved()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async void RemoveScript_WithIncludes_ScriptRemoved()
    {
        throw new NotImplementedException();
    }

    #endregion
}