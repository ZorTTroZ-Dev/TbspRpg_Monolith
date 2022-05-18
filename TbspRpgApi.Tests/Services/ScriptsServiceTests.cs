using System;
using System.Collections.Generic;
using TbspRpgDataLayer.Entities;
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
}