using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
using TbspRpgSettings.Settings;
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
            adventureObject = new ObjectViewModel()
            {
                Id = exceptionId,
                AdventureId = Guid.NewGuid(),
                Name = "test",
                Type = "generic",
                Locations = new List<LocationViewModel>()
            },
            nameSource = new SourceViewModel()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
            },
            descriptionSource = new SourceViewModel()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
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
            adventureObject = new ObjectViewModel()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test",
                Type = "generic",
                Locations = new List<LocationViewModel>()
            },
            nameSource = new SourceViewModel()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
            },
            descriptionSource = new SourceViewModel()
            {
                Id = Guid.NewGuid(),
                Language = Languages.ENGLISH
            }
        });
    }
    
    #endregion
    
    #region GetObjectsByLocation

    [Fact]
    public async void GetsObjectsByLocation_HasObjects_ReturnList()
    {
        // arrange
        var testObjects = new List<AdventureObject>()
        {
            new AdventureObject()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test",
                Locations = new List<Location>()
                {
                    new Location()
                    {
                        Id = Guid.NewGuid(),
                        Name = "tl"
                    }
                }
            },
            new AdventureObject()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test two",
                Locations = new List<Location>()
                {
                    new Location()
                    {
                        Id = Guid.NewGuid(),
                        Name = "tl2"
                    }
                }
            }
        };
        var service = CreateObjectsService(testObjects);
        
        // act
        var objects = await service.GetObjectsByLocation(testObjects[0].Locations.First().Id);
        
        // assert
        Assert.Single(objects);
        Assert.Equal("test", objects[0].Name);
        Assert.Single(objects[0].Locations);
    }
    
    [Fact]
    public async void GetObjectsByLocation_NoObjects_ReturnEmpty()
    {
        // arrange
        var testObjects = new List<AdventureObject>()
        {
            new AdventureObject()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test",
                Locations = new List<Location>()
                {
                    new Location()
                    {
                        Id = Guid.NewGuid(),
                        Name = "tl"
                    }
                }
            },
            new AdventureObject()
            {
                Id = Guid.NewGuid(),
                AdventureId = Guid.NewGuid(),
                Name = "test two",
                Locations = new List<Location>()
                {
                    new Location()
                    {
                        Id = Guid.NewGuid(),
                        Name = "tl2"
                    }
                }
            }
        };
        var service = CreateObjectsService(testObjects);
        
        // act
        var objects = await service.GetObjectsByLocation(Guid.NewGuid());
        
        // assert
        Assert.Empty(objects);
    }

    #endregion
}