using System;
using System.Collections.Generic;
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
                function run()
		            result = 'foo'
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

    #endregion
}