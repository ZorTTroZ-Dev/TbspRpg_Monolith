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
            ReturnType = "void",
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
            ReturnType = "void",
            Type = ScriptTypes.LuaScript,
            Includes = new List<Script>()
            {
                new Script()
                {
                    Id = Guid.NewGuid(),
                    Name = "test_base",
                    Content = "print('base banana');",
                    ReturnType = "void",
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
}