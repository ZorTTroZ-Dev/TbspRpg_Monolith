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

public class ObjectsControllerTests: ApiTest
{
    private ObjectsController CreateController(ICollection<AdventureObject> objects, Guid? exceptionId = null)
    {
        var service = CreateObjectsService(objects, exceptionId);
        return new ObjectsController(service,
            MockPermissionService(),
            NullLogger<ScriptsController>.Instance);
    }
    
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
        var controller = CreateController(testObjects);
        
        // act
        var response = await controller.GetObjectsForAdventure(testObjects[0].AdventureId);
        
        // assert
        var okObjectResult = response as OkObjectResult;
        Assert.NotNull(okObjectResult);
        var objectViewModels = okObjectResult.Value as List<ObjectViewModel>;
        Assert.NotNull(objectViewModels);
        Assert.Single(objectViewModels);
    }
    
    [Fact]
    public async void GetsObjectsForAdventure_NoObjects_ReturnEmpty()
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
        var controller = CreateController(testObjects);
        
        // act
        var response = await controller.GetObjectsForAdventure(Guid.NewGuid());
        
        // assert
        var okObjectResult = response as OkObjectResult;
        Assert.NotNull(okObjectResult);
        var objectViewModels = okObjectResult.Value as List<ObjectViewModel>;
        Assert.NotNull(objectViewModels);
        Assert.Empty(objectViewModels);
    }

    #endregion
    
    #region DeleteAdventureObject

    [Fact]
    public async void DeleteAdventureObject_InvalidId_BadRequest()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var controller = CreateController(new List<AdventureObject>(), exceptionId);
            
        // act
        var response = await controller.DeleteObject(exceptionId);
            
        // assert
        var badRequestResult = response as BadRequestObjectResult;
        Assert.NotNull(badRequestResult);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async void DeleteAdventureObject_Valid_ReturnOk()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var controller = CreateController(new List<AdventureObject>(), exceptionId);
            
        // act
        var response = await controller.DeleteObject(Guid.NewGuid());
            
        // assert
        var okObjectResult = response as OkResult;
        Assert.NotNull(okObjectResult);
    }

    #endregion

    #region UpdateObject

    [Fact]
    public async void UpdateObject_Valid_ReturnOk()
    {
        // arrange
        var controller = CreateController(new List<AdventureObject>(), Guid.NewGuid());
        
        // act
        var response = await controller.UpdateObject(
            new ObjectUpdateRequest()
            {
                obj = new ObjectViewModel()
                {
                    Id = Guid.NewGuid(),
                    AdventureId = Guid.NewGuid(),
                    Name = "script",
                    Locations = new List<LocationViewModel>()
                }
            });
        
        // assert
        var okObjectResult = response as OkObjectResult;
        Assert.NotNull(okObjectResult);
    }

    [Fact]
    public async void UpdateObject_UpdateFails_ReturnBadRequest()
    {
        // arrange
        var exceptionId = Guid.NewGuid();
        var controller = CreateController(new List<AdventureObject>(), exceptionId);
        
        // act
        var response = await controller.UpdateObject(
            new ObjectUpdateRequest()
            {
                obj = new ObjectViewModel()
                {
                    Id = exceptionId,
                    AdventureId = Guid.NewGuid(),
                    Name = "script",
                    Type = AdventureObjectTypes.Generic
                }
            });
        
        // assert
        var badRequestResult = response as BadRequestObjectResult;
        Assert.NotNull(badRequestResult);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    #endregion
}