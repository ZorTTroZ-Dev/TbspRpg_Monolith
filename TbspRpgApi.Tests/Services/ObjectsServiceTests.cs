using System;
using System.Collections.Generic;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Services;

public class ObjectsServiceTests: ApiTest
{
    #region GetObjectsForAdventure

    [Fact]
    public async void GetsObjectsForAdventure_HasObjects_ReturnList()
    {
        // arrange
        var testObjects = new List<AdventureObject>()
        {
            new AdventureObject()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test"
            },
            new AdventureObject()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test two"
            }
        };
        var service = CreateObjectsService(testObjects);
        
        // act
        var objects = await service.GetObjectsForAdventure(testObjects[0].AdventureId);
        
        // assert
        Assert.Single(objects);
        Assert.Equal("test", objects[0].Name);
    }
    
    [Fact]
    public async void GetObjectsForAdventure_NoObjects_ReturnEmpty()
    {
        // arrange
        var testObjects = new List<AdventureObject>()
        {
            new AdventureObject()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test"
            },
            new AdventureObject()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test two"
            }
        };
        var service = CreateObjectsService(testObjects);
        
        // act
        var objects = await service.GetObjectsForAdventure(Guid.NewGuid());
        
        // assert
        Assert.Empty(objects);
    }

    #endregion
}