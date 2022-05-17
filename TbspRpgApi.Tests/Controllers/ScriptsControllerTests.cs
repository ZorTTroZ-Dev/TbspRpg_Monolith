using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using TbspRpgApi.Controllers;
using TbspRpgDataLayer.Entities;
using Xunit;

namespace TbspRpgApi.Tests.Controllers;

public class ScriptsControllerTests: ApiTest
{
    private ScriptsController CreateController(ICollection<Script> scripts)
    {
        var service = CreateScriptsService(scripts);
        return new ScriptsController(service,
            MockPermissionService(),
            NullLogger<ScriptsController>.Instance);
    }
    
    #region GetScriptsForAdventure

    [Fact]
    public async void GetsScriptsForAdventure_HasScripts_ReturnList()
    {
        
    }
    
    [Fact]
    public async void GetsScriptsForAdventure_NoScripts_ReturnEmpty()
    {
        
    }

    #endregion
}