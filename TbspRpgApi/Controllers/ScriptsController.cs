using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.RequestModels;
using TbspRpgApi.Services;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScriptsController : BaseController
{
    private readonly IScriptsService _scriptsService;
    private readonly IPermissionService _permissionService;
    private readonly ILogger<ScriptsController> _logger;

    public ScriptsController(
        IScriptsService scriptsService,
        IPermissionService permissionService,
        ILogger<ScriptsController> logger)
    {
        _scriptsService = scriptsService;
        _permissionService = permissionService;
        _logger = logger;
    }
    
    [HttpGet("adventure/{adventureId:guid}"), Authorize]
    public async Task<IActionResult> GetScriptsForAdventure(Guid adventureId)
    {
        var canAccessAdventure = await _permissionService.CanWriteAdventure(
            GetUserId().GetValueOrDefault(),
            adventureId);
        if(!canAccessAdventure)
        {
            return BadRequest(new { message = NotYourAdventureErrorMessage });
        }

        var scripts = await _scriptsService.GetScriptsForAdventure(adventureId);
        return Ok(scripts);
    }
    
    [HttpPut, Authorize]
    public async Task<IActionResult> UpdateScript([FromBody] ScriptUpdateRequest scriptUpdateRequest)
    {
        var canAccessAdventure = await _permissionService.CanWriteAdventure(
            GetUserId().GetValueOrDefault(),
            scriptUpdateRequest.script.AdventureId);
        if (!canAccessAdventure)
        {
            return BadRequest(new { message = NotYourAdventureErrorMessage });
        }
        
        // make sure the script doesn't include itself
        if (scriptUpdateRequest.script.Includes.FirstOrDefault(script =>
                script.Id == scriptUpdateRequest.script.Id) != null)
        {
            return BadRequest(new { message = "script can not include itself" });
        }

        try
        {
            await _scriptsService.UpdateScript(scriptUpdateRequest);
            return Ok(null);
        }
        catch (Exception ex)
        {
            return BadRequest((new {message = ex.Message}));
        }
    }
}