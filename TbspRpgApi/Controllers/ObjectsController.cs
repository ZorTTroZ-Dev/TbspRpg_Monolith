using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.RequestModels;
using TbspRpgApi.Services;

namespace TbspRpgApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ObjectsController : BaseController
{
    private readonly IObjectsService _objectsService;
    private readonly IPermissionService _permissionService;
    private readonly ILogger<ScriptsController> _logger;

    public ObjectsController(
        IObjectsService objectsService,
        IPermissionService permissionService,
        ILogger<ScriptsController> logger)
    {
        _objectsService = objectsService;
        _permissionService = permissionService;
        _logger = logger;
    }

    [HttpGet("adventure/{adventureId:guid}"), Authorize]
    public async Task<IActionResult> GetObjectsForAdventure(Guid adventureId)
    {
        var canAccessAdventure = await _permissionService.CanWriteAdventure(
            GetUserId().GetValueOrDefault(),
            adventureId);
        if(!canAccessAdventure)
        {
            return BadRequest(new { message = NotYourAdventureErrorMessage });
        }
        
        var objects = await _objectsService.GetObjectsForAdventure(adventureId);
        return Ok(objects);
    }
    
    [HttpGet("location/{locationId:guid}"), Authorize]
    public async Task<IActionResult> GetObjectsByLocation(Guid locationId)
    {
        var canAccessLocation = await _permissionService.CanWriteLocation(
            GetUserId().GetValueOrDefault(),
            locationId);
        if(!canAccessLocation)
        {
            return BadRequest(new { message = NotYourAdventureErrorMessage });
        }
        
        var objects = await _objectsService.GetObjectsByLocation(locationId);
        return Ok(objects);
    }
    
    [HttpDelete("{objectId:guid}"), Authorize]
    public async Task<IActionResult> DeleteObject(Guid objectId)
    {
        var canDeleteObject = await _permissionService.CanDeleteObject(
            GetUserId().GetValueOrDefault(), objectId);
        // make sure the user either is the adventure's owner or the user is an admin or has write adventure permission
        if(!canDeleteObject)
            return BadRequest(new { message = NotYourAdventureErrorMessage });
            
        try
        {
            await _objectsService.DeleteObject(objectId);
            return Ok();
        }
        catch
        {
            return BadRequest(new { message = "couldn't delete object" });
        }
    }
    
    [HttpPut, Authorize]
    public async Task<IActionResult> UpdateObject([FromBody] ObjectUpdateRequest objectUpdateRequest)
    {
        var canAccessAdventure = await _permissionService.CanWriteAdventure(
            GetUserId().GetValueOrDefault(),
            objectUpdateRequest.obj.AdventureId);
        if (!canAccessAdventure)
        {
            return BadRequest(new { message = NotYourAdventureErrorMessage });
        }
        try
        {
            await _objectsService.UpdateObject(objectUpdateRequest);
            return Ok(null);
        }
        catch (Exception ex)
        {
            return BadRequest((new {message = ex.Message}));
        }
    }
}