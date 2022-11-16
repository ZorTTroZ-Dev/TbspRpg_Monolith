using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.JwtAuthorization;
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
    
    [HttpDelete("{objectId:guid}"), Authorize]
    public async Task<IActionResult> DeleteObject(Guid objectId)
    {
        // var canDeleteScript = await _permissionService.CanDeleteScript(GetUserId().GetValueOrDefault(), scriptId);
        // // make sure the user either is the adventure's owner or the user is an admin or has write adventure permission
        // if(!canDeleteScript)
        //     return BadRequest(new { message = NotYourAdventureErrorMessage });
        //     
        // try
        // {
        //     await _scriptsService.DeleteScript(scriptId);
        //     return Ok();
        // }
        // catch
        // {
        //     return BadRequest(new { message = "couldn't delete script" });
        // }
        return Ok();
    }
    
    [HttpPut, Authorize]
    public async Task<IActionResult> UpdateObject(/*[FromBody] ScriptUpdateRequest scriptUpdateRequest*/)
    {
        // var canAccessAdventure = await _permissionService.CanWriteAdventure(
        //     GetUserId().GetValueOrDefault(),
        //     scriptUpdateRequest.script.AdventureId);
        // if (!canAccessAdventure)
        // {
        //     return BadRequest(new { message = NotYourAdventureErrorMessage });
        // }
        //
        // // make sure the script doesn't include itself
        // if (scriptUpdateRequest.script.Includes.FirstOrDefault(script =>
        //         script.Id == scriptUpdateRequest.script.Id) != null)
        // {
        //     return BadRequest(new { message = "script can not include itself" });
        // }
        //
        // try
        // {
        //     await _scriptsService.UpdateScript(scriptUpdateRequest);
        //     return Ok(null);
        // }
        // catch (Exception ex)
        // {
        //     return BadRequest((new {message = ex.Message}));
        // }
        return Ok();
    }
}