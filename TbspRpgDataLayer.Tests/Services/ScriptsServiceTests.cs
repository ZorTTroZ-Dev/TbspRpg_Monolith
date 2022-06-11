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
    
    #region GetScriptsForAdventure

    [Fact]
    public async void GetScriptsForAdventure_ValidId_ReturnsScripts()
    {
        // arrange
        var testScripts = new List<Script>()
        {
            new Script()
            {
                Id = Guid.NewGuid(),
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid(),
                    Name = "test"
                }
            },
            new Script()
            {
                Id = Guid.NewGuid(),
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid(),
                    Name = "test two"
                }
            }
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.Scripts.AddRangeAsync(testScripts);
        await context.SaveChangesAsync();
        var service = CreateService(context);
        
        // act
        var scripts = await service.GetScriptsForAdventure(testScripts[0].Adventure.Id);
        
        // assert
        Assert.Single(scripts);
        Assert.Equal("test", scripts[0].Adventure.Name);
    }

    [Fact]
    public async void GetScriptsForAdventure_InvalidId_ReturnEmptyList()
    {
        // arrange
        var testScripts = new List<Script>()
        {
            new Script()
            {
                Id = Guid.NewGuid(),
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid(),
                    Name = "test"
                }
            },
            new Script()
            {
                Id = Guid.NewGuid(),
                Adventure = new Adventure()
                {
                    Id = Guid.NewGuid(),
                    Name = "test two"
                }
            }
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.Scripts.AddRangeAsync(testScripts);
        await context.SaveChangesAsync();
        var service = CreateService(context);
        
        // act
        var scripts = await service.GetScriptsForAdventure(Guid.NewGuid());
        
        // assert
        Assert.Empty(scripts);
    }

    #endregion
    
    #region AddScript

    [Fact]
    public async void AddScript_ScriptAdded()
    {
        // arrange
        await using var context = new DatabaseContext(DbContextOptions);
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script"
        };
        var service = CreateService(context);
        
        // act
        await service.AddScript(testScript);
        await service.SaveChanges();
        
        // assert
        Assert.Single(context.Scripts);
    }

        #endregion
        
    #region RemoveScript

    [Fact]
    public async void RemoveScript_ScriptRemoved()
    {
        // arrange
        await using var context = new DatabaseContext(DbContextOptions);
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script"
        };
        var testScriptTwo = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script Two"
        };
        context.Scripts.Add(testScript);
        context.Scripts.Add(testScriptTwo);
        await context.SaveChangesAsync();
        var service = CreateService(context);

        // act
        service.RemoveScript(testScript);
        await service.SaveChanges();
        
        // assert
        Assert.Single(context.Scripts);
    }
    
    #endregion

    #region RemoveScripts

    [Fact]
    public async void RemoveScripts_ScriptsRemoved()
    {
        // arrange
        await using var context = new DatabaseContext(DbContextOptions);
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script"
        };
        var testScriptTwo = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script Two"
        };
        context.Scripts.Add(testScript);
        context.Scripts.Add(testScriptTwo);
        await context.SaveChangesAsync();
        var service = CreateService(context);
        
        // act
        service.RemoveScripts(new List<Script>() { testScript, testScriptTwo});
        await service.SaveChanges();
        
        // assert
        Assert.Empty(context.Scripts);
    }

    #endregion

    #region RemoveAllScriptsForAdventure

    [Fact]
    public async void RemoveAllScriptsForAdventure_ScriptsRemoved()
    {
        // arrange
        var testAdventure = new Adventure()
        {
            Id = Guid.NewGuid(),
            Name = "test adventure"
        };
        var testScript = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script",
            Adventure = testAdventure
        };
        var testScriptTwo = new Script()
        {
            Id = Guid.NewGuid(),
            Name = "test script two",
            Adventure = testAdventure
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.Adventures.AddRangeAsync(testAdventure);
        await context.Scripts.AddRangeAsync(testScript, testScriptTwo);
        await context.SaveChangesAsync();
        var service = CreateService(context);
        
        // act
        await service.RemoveAllScriptsForAdventure(testAdventure.Id);
        await service.SaveChanges();
        
        // assert
        Assert.Empty(context.Scripts);
    }

    #endregion
}