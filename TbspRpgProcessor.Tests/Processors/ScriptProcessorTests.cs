using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLua.Exceptions;
using TbspRpgDataLayer.Entities;
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
}