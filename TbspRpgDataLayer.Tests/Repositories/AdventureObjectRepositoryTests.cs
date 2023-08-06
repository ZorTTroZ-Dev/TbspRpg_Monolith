using System;
using System.Collections.Generic;
using System.Linq;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Repositories;
using TbspRpgSettings.Settings;
using Xunit;

namespace TbspRpgDataLayer.Tests.Repositories;

public class AdventureObjectRepositoryTests: InMemoryTest
{
    public AdventureObjectRepositoryTests() : base("AdventureObjectRepositoryTests")
    {
    }

    #region GetAdventureObjectById

    [Fact]
    public async void GetAdventureObjectById_InvalidId_ReturnNull()
    {
        // arrange
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.AdventureObjects.AddAsync(testObject);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectRepository(context);

        // act
        var adventureObject = await repository.GetAdventureObjectById(Guid.NewGuid());

        // assert
        Assert.Null(adventureObject);
    }

    [Fact]
    public async void GetAdventureObjectById_Valid_ReturnAdventureObject()
    {
        // arrange
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.AdventureObjects.AddAsync(testObject);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectRepository(context);

        // act
        var adventureObject = await repository.GetAdventureObjectById(testObject.Id);

        // assert
        Assert.NotNull(adventureObject);
        Assert.Equal(testObject.Id, adventureObject.Id);
    }

    #endregion

    #region GetAdventureObjectsForAdventure

    [Fact]
    public async void GetAdventureObjectsForAdventure_ValidId_ReturnsAdventureObjects()
    {
        // arrange
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            }
        };
        var testObjectTwo = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object two",
            Description = "test object two",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test two"
            }
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.AdventureObjects.AddAsync(testObject);
        await context.AdventureObjects.AddAsync(testObjectTwo);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectRepository(context);

        // act
        var adventureObjects = 
            await repository.GetAdventureObjectsForAdventure(testObject.Adventure.Id);
        
        // assert
        Assert.Single(adventureObjects);
        Assert.Equal("test", adventureObjects[0].Adventure.Name);
    }

    [Fact]
    public async void GetAdventureObjectsForAdventure_InvalidId_ReturnEmptyList()
    {
        // arrange
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            }
        };
        var testObjectTwo = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object two",
            Description = "test object two",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test two"
            }
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.AdventureObjects.AddAsync(testObject);
        await context.AdventureObjects.AddAsync(testObjectTwo);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectRepository(context);
        
        // act
        var adventureObjects = await repository.GetAdventureObjectsForAdventure(Guid.NewGuid());
        
        // assert
        Assert.Empty(adventureObjects);
    }

    #endregion
    
    #region AddAdventureObject

    [Fact]
    public async void AddAdventureObject_AdventureObjectAdded()
    {
        // arrange
        await using var context = new DatabaseContext(DbContextOptions);
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            }
        };
        var repository = new AdventureObjectRepository(context);
        
        // act
        await repository.AddAdventureObject(testObject);
        await repository.SaveChanges();
        
        // assert
        Assert.Single(context.AdventureObjects);
    }

    #endregion
        
    #region RemoveAdventureObject

    [Fact]
    public async void RemoveAdventureObject_AdventureObjectRemoved()
    {
        // arrange
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            }
        };
        var testObjectTwo = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object two",
            Description = "test object two",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test two"
            }
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.AdventureObjects.AddAsync(testObject);
        await context.AdventureObjects.AddAsync(testObjectTwo);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectRepository(context);

        // act
        repository.RemoveAdventureObject(testObject);
        await repository.SaveChanges();
        
        // assert
        Assert.Single(context.AdventureObjects);
    }
    
    #endregion

    #region RemoveAdventureObjects

    [Fact]
    public async void RemoveAdventureObjects_AdventureObjectsRemoved()
    {
        // arrange
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            }
        };
        var testObjectTwo = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object two",
            Description = "test object two",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test two"
            }
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.AdventureObjects.AddAsync(testObject);
        await context.AdventureObjects.AddAsync(testObjectTwo);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectRepository(context);
        
        // act
        repository.RemoveAdventureObjects(new List<AdventureObject>() { testObject, testObjectTwo });
        await repository.SaveChanges();
        
        // assert
        Assert.Empty(context.AdventureObjects);
    }

    #endregion
    
    #region GetAdventureObjectsByLocation

    [Fact]
    public async void GetAdventureObjectsForLocation_ValidId_ReturnsAdventureObjects()
    {
        // arrange
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            },
            Locations = new List<Location>()
            {
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation"
                },
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation2"
                }
            }
        };
        var testObjectTwo = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object two",
            Description = "test object two",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test two"
            },
            Locations = new List<Location>()
            {
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation3"
                },
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation4"
                }
            }
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.AdventureObjects.AddAsync(testObject);
        await context.AdventureObjects.AddAsync(testObjectTwo);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectRepository(context);

        // act
        var adventureObjects = 
            await repository.GetAdventureObjectsByLocation(testObject.Locations.First().Id);
        
        // assert
        Assert.Single(adventureObjects);
        Assert.Equal("test", adventureObjects[0].Adventure.Name);
        Assert.Equal(2, adventureObjects[0].Locations.Count);
    }
    
    [Fact]
    public async void GetAdventureObjectsForLocation_ValidId_ReturnsMultipleAdventureObjects()
    {
        // arrange
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            },
            Locations = new List<Location>()
            {
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation"
                },
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation2"
                }
            }
        };
        var testObjectTwo = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object two",
            Description = "test object two",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test two"
            },
            Locations = testObject.Locations
        };
        await using var context = new DatabaseContext(DbContextOptions);
        await context.AdventureObjects.AddAsync(testObject);
        await context.AdventureObjects.AddAsync(testObjectTwo);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectRepository(context);

        // act
        var adventureObjects = 
            await repository.GetAdventureObjectsByLocation(testObject.Locations.First().Id);
        
        // assert
        Assert.Equal(2, adventureObjects.Count);
        Assert.Equal(2, adventureObjects[0].Locations.Count);
        Assert.Equal(2, adventureObjects[1].Locations.Count);
    }

    [Fact]
    public async void GetAdventureObjectsForLocation_InvalidId_ReturnEmptyList()
    {
        // arrange
        var testObject = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object",
            Description = "test object",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test"
            },
            Locations = new List<Location>()
            {
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation"
                },
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation2"
                }
            }
        };
        var testObjectTwo = new AdventureObject()
        {
            Id = Guid.NewGuid(),
            Name = "test object two",
            Description = "test object two",
            Type = AdventureObjectTypes.Generic,
            Adventure = new Adventure()
            {
                Id = Guid.NewGuid(),
                Name = "test two"
            },
            Locations = new List<Location>()
            {
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation3"
                },
                new Location()
                {
                    Id = Guid.NewGuid(),
                    Name = "testlocation4"
                }
            }
        };
        
        await using var context = new DatabaseContext(DbContextOptions);
        await context.AdventureObjects.AddAsync(testObject);
        await context.AdventureObjects.AddAsync(testObjectTwo);
        await context.SaveChangesAsync();
        var repository = new AdventureObjectRepository(context);
        
        // act
        var adventureObjects = await repository.GetAdventureObjectsByLocation(Guid.NewGuid());
        
        // assert
        Assert.Empty(adventureObjects);
    }

    #endregion
}