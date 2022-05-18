using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Controllers;

public class ScriptsControllerTests: ApiTest
{
    private ScriptsController CreateController(ICollection<Script> scripts)
    {
        var service = CreateScriptsService(scripts);
        return new ScriptsController(service,
            MockPermissionService(),
            NullLogger<ScriptsController>.Instance);
    }
    
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
        var controller = CreateController(testScripts);
        
        // act
        var response = await controller.GetScriptsForAdventure(testScripts[0].AdventureId);
        
        // assert
        var okObjectResult = response as OkObjectResult;
        Assert.NotNull(okObjectResult);
        var scriptViewModels = okObjectResult.Value as List<ScriptViewModel>;
        Assert.NotNull(scriptViewModels);
        Assert.Single(scriptViewModels);
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
        var controller = CreateController(testScripts);
        
        // act
        var response = await controller.GetScriptsForAdventure(Guid.NewGuid());
        
        // assert
        var okObjectResult = response as OkObjectResult;
        Assert.NotNull(okObjectResult);
        var scriptViewModels = okObjectResult.Value as List<ScriptViewModel>;
        Assert.NotNull(scriptViewModels);
        Assert.Empty(scriptViewModels);
    }

    #endregion
}