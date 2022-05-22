using System;
using System.Collections.Generic;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories;

public class ScriptsRepositoryTests: InMemoryTest
{
    public ScriptsRepositoryTests() : base("ScriptsRepositoryTests")
    {
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
        var repository = new ScriptsRepository(context);

        // act
        var script = await repository.GetScriptById(Guid.NewGuid());

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
        var repository = new ScriptsRepository(context);

        // act
        var script = await repository.GetScriptById(testScript.Id);
        
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
        var repository = new ScriptsRepository(context);
        
        // act
        var scripts = await repository.GetScriptsForAdventure(testScripts[0].Adventure.Id);
        
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
        var repository = new ScriptsRepository(context);
        
        // act
        var scripts = await repository.GetScriptsForAdventure(Guid.NewGuid());
        
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
        var repository = new ScriptsRepository(context);
        
        // act
        await repository.AddScript(testScript);
        await repository.SaveChanges();
        
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
        var repository = new ScriptsRepository(context);

        // act
        repository.RemoveScript(testScript);
        await repository.SaveChanges();
        
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
        var repository = new ScriptsRepository(context);
        
        // act
        repository.RemoveScripts(new List<Script>() { testScript, testScriptTwo});
        await repository.SaveChanges();
        
        // assert
        Assert.Empty(context.Scripts);
    }

    #endregion
}