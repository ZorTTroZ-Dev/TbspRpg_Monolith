using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgApi.Tests.Controllers;

public class ScriptsControllerTests: ApiTest
{
    private ScriptsController CreateController(ICollection<Script> scripts, Guid? exceptionId = null)
    {
        var service = CreateScriptsService(scripts, exceptionId);
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
    
    #region UpdateScript

    [Fact]
    public async void UpdateScript_Valid_ReturnOk()
    {
        // arrange
        var controller = CreateController(new List<Script>(), Guid.NewGuid());
        
        // act
        var response = await controller.UpdateScript(
            new ScriptUpdateRequest()
            {
                script = new ScriptViewModel()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = Guid.NewGuid(),
                    Content = "content",
                    Type = ScriptTypes.LuaScript,
                    Name = "script",
                    Includes = new List<ScriptViewModel>()
                }
            });
        
        // assert
        var okObjectResult = response as OkObjectResult;
        Assert.NotNull(okObjectResult);
    }

    [Fact]
    public async void UpdateScript_UpdateFails_ReturnBadRequest()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var controller = CreateController(new List<Script>(), exceptionId);
        
        // act
        var response = await controller.UpdateScript(
            new ScriptUpdateRequest()
            {
                script = new ScriptViewModel()
                {
                    Id = exceptionId,
                    AdventureId = Guid.NewGuid(),
                    Content = "content",
                    Type = ScriptTypes.LuaScript,
                    Name = "script",
                    Includes = new List<ScriptViewModel>()
                }
            });
        
        // assert
        var badRequestResult = response as BadRequestObjectResult;
        Assert.NotNull(badRequestResult);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
    
    [Fact]
    public async void UpdateScript_ScriptIncludesSelf_ReturnBadRequest()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var controller = CreateController(new List<Script>(), exceptionId);
        var testScriptRequest = new ScriptUpdateRequest()
        {
            script = new ScriptViewModel()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Content = "content",
                Type = ScriptTypes.LuaScript,
                Name = "script",
                Includes = new List<ScriptViewModel>()
            }
        };
        testScriptRequest.script.Includes.Add(new ScriptViewModel()
        {
            Id = testScriptRequest.script.Id,
            AdventureId = Guid.NewGuid(),
            Content = "content",
            Type = ScriptTypes.LuaScript,
            Name = "script",
            Includes = new List<ScriptViewModel>()
        });
        
        // act
        var response = await controller.UpdateScript(testScriptRequest);
        
        // assert
        var badRequestResult = response as BadRequestObjectResult;
        Assert.NotNull(badRequestResult);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    #endregion
    
    #region DeleteScript

    [Fact]
    public async void DeleteScript_Valid_NoException()
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
        var response = await controller.DeleteScript(testScripts[0].Id);
        
        // assert
        var okResult = response as OkResult;
        Assert.NotNull(okResult);
    }
    
    [Fact]
    public async void DeleteScript_DeleteFails_ExceptionThrown()
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
        var exceptionId = Guid.NewGuid();
        var controller = CreateController(testScripts, exceptionId);
        
        // act
        var response = await controller.DeleteScript(exceptionId);
        
        // assert
        var badRequestResult = response as BadRequestObjectResult;
        Assert.NotNull(badRequestResult);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    #endregion
}