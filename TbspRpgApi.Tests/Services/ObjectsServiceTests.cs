using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
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
    
    #region RemoveObject

    [Fact]
    public async void RemoveAdventureObject_InvalidId_ExceptionThrown()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var service = CreateObjectsService(new List<AdventureObject>(), exceptionId);
            
        // act
        Task Act() => service.DeleteObject(exceptionId);
            
        // assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }

    [Fact]
    public async void RemoveAdventureObject_Valid_AdventureRemoved()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var service = CreateObjectsService(new List<AdventureObject>(), exceptionId);
            
        // act
        await service.DeleteObject(Guid.NewGuid());
    }
        
    #endregion
    
    #region UpdateAdventureObject
    
    [Fact]
    public async void UpdateAdventureObject_InvalidId_ExceptionThrown()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var service = CreateObjectsService(new List<AdventureObject>(), exceptionId);
            
        // act
        Task Act() => service.UpdateObject(new ObjectUpdateRequest()
        {
            obj = new ObjectViewModel()
            {
                Id = exceptionId,
                AdventureId = Guid.NewGuid(),
                Name = "test",
                Type = "generic",
                Locations = new List<LocationViewModel>()
            }
        });
            
        // assert
        await Assert.ThrowsAsync<ArgumentException>(Act);
    }

    [Fact]
    public async void UpdateAdventureObject_Valid_AdventureObjectUpdated()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var service = CreateObjectsService(new List<AdventureObject>(), exceptionId);
            
        // act
        await service.UpdateObject(new ObjectUpdateRequest()
        {
            obj = new ObjectViewModel()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test",
                Type = "generic",
                Locations = new List<LocationViewModel>()
            }
        });
    }
    
    #endregion
}