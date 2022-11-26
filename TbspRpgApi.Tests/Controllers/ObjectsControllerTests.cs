using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;
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
}