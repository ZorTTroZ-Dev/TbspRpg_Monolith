using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgApi.Tests.Services;

public class ScriptsServiceTests: ApiTest
{
    #region GetScriptsForAdventure

    [Fact]
    public async void GetsScriptsForAdventure_HasScripts_ReturnList()
    {
        // arrange
        var testScripts = new List<Script>()
        {
            new Script()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test"
            },
            new Script()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test two"
            }
        };
        var service = CreateScriptsService(testScripts);
        
        // act
        var scripts = await service.GetScriptsForAdventure(testScripts[0].AdventureId);
        
        // assert
        Assert.Single(scripts);
        Assert.Equal("test", scripts[0].Name);
    }
    
    [Fact]
    public async void GetsScriptsForAdventure_NoScripts_ReturnEmpty()
    {
        // arrange
        var testScripts = new List<Script>()
        {
            new Script()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test"
            },
            new Script()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test two"
            }
        };
        var service = CreateScriptsService(testScripts);
        
        // act
        var scripts = await service.GetScriptsForAdventure(Guid.NewGuid());
        
        // assert
        Assert.Empty(scripts);
    }

    #endregion
    
    #region UpdateScript
    
    [Fact]
    public async void UpdateScript_Valid_ScriptUpdated()
    {
        // arrange
        var service = CreateScriptsService(new List<Script>(), Guid.NewGuid());
        
        // act
        await service.UpdateScript(new ScriptUpdateRequest()
        {
            script = new ScriptViewModel()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Content = "content",
                Type = ScriptTypes.LuaScript,
                Name = "script",
                Includes = new List<ScriptViewModel>()
                {
                    new ScriptViewModel()
                    {
                        Id = Guid.NewGuid(),
                        Name = "include",
                        AdventureId = Guid.NewGuid(),
                        Content = "base",
                        Type = ScriptTypes.LuaScript,
                        Includes = new List<ScriptViewModel>()
                    }
                }
            }
        });

        // assert
    }
    
    [Fact]
    public async void UpdateScript_InValid_ExceptionThrown()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var service = CreateScriptsService(new List<Script>(), exceptionId);
        
        // act
        Task Act() => service.UpdateScript(new ScriptUpdateRequest()
        {
            script = new ScriptViewModel()
            {
                Id = exceptionId,
                AdventureId = Guid.NewGuid(),
                Content = "content",
                Type = ScriptTypes.LuaScript,
                Name = "script",
                Includes = new List<ScriptViewModel>()
                {
                    new ScriptViewModel()
                    {
                        Id = Guid.NewGuid(),
                        Name = "include",
                        AdventureId = Guid.NewGuid(),
                        Content = "base",
                        Type = ScriptTypes.LuaScript,
                        Includes = new List<ScriptViewModel>()
                    }
                }
            }
        });
        
        // assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }

    #endregion
}