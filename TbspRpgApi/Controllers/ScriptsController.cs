using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.Services;

namespace TbspRpgApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScriptsController : BaseController
{
    private readonly IScriptsService _scriptsService;
    private readonly IPermissionService _permissionService;
    private readonly ILogger<SourcesController> _logger;

    public ScriptsController(
        IScriptsService scriptsService,
        IPermissionService permissionService,
        ILogger<SourcesController> logger)
    {
        _scriptsService = scriptsService;
        _permissionService = permissionService;
        _logger = logger;
    }
    
    [HttpGet("adventure/{adventureId:guid}")]
    [Authorize]
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
}