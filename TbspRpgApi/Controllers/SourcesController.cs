using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.RequestModels;
using TbspRpgApi.Services;

namespace TbspRpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SourcesController : BaseController
    {
        private readonly ISourcesService _sourcesService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<SourcesController> _logger;

        public SourcesController(ISourcesService sourcesService,
            IPermissionService permissionService,
            ILogger<SourcesController> logger)
        {
            _sourcesService = sourcesService;
            _permissionService = permissionService;
            _logger = logger;
        }
        
        [HttpGet("adventure/{adventureId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetSourcesForAdventure(Guid adventureId, [FromQuery]SourceFilterRequest filters)
        {
            var canAccessAdventure = await _permissionService.CanReadAdventure(
                GetUserId().GetValueOrDefault(),
                adventureId);
            if(!canAccessAdventure)
            {
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            }

            // if language is missing throw an exception
            if (filters.Language == null)
            {
                return BadRequest(new {message = "language required"});
            }
            
            // key is missing call a different method one that returns all sources for an adventure
            if (filters.Key == null)
            {
                return BadRequest(new {message = "operation not supported yet."});
            }
            
            var source = await _sourcesService.GetSourceForKey(filters.Key.GetValueOrDefault(), adventureId, filters.Language);
            return Ok(source);
        }
    }
}