using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgDataLayer.Services;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgDataLayer.Tests.Services;

public class ScriptsServiceTests: InMemoryTest
{
    public ScriptsServiceTests() : base("ScriptsServiceTests")
    {
    }
    
    private static IScriptsService CreateService(DatabaseContext context)
    {
        return new ScriptsService(
            new ScriptsRepository(context),
            NullLogger<ScriptsService>.Instance);
    }
    
    #region GetScriptById

    [Fact]
    public async void GetScriptById_InvalidId_ReturnNull()
    {
        // arrange
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Content = "print('banana');",
            Type = ScriptTypes.LuaScript,
            Includes = new List<Script>()
        };
        await using var context = new DatabaseContext(DbContextOptions);
        context.Scripts.Add(testScript);
        await context.SaveChangesAsync();
        var service = CreateService(context);

        // act
        var script = await service.GetScriptById(Guid.NewGuid());

        // assert
        Assert.Null(script);
    }

    [Fact]
    public async void GetScriptById_Valid_ReturnScriptIncludeIncludes()
    {
        // arrange
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test",
            Content = "print('banana');",
            Type = ScriptTypes.LuaScript,
            Includes = new List<Script>()
            {
                new Script()
                {
                    Id = Guid.NewGuid(),
                    Name = "test_base",
                    Content = "print('base banana');",
                    Type = ScriptTypes.LuaScript,
                }
            }
        };
        await using var context = new DatabaseContext(DbContextOptions);
        context.Scripts.Add(testScript);
        await context.SaveChangesAsync();
        var service = CreateService(context);

        // act
        var script = await service.GetScriptById(testScript.Id);
        
        // assert
        Assert.NotNull(script);
        Assert.Equal(testScript.Id, script.Id);
        Assert.Single(script.Includes);
    }

    #endregion
}