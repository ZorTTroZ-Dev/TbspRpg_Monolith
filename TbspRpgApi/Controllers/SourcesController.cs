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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSourceForKey([FromQuery] SourceFilterRequest filters)
        {
            // this will only work with the empty source id
            if (filters.Key != Guid.Empty)
            {
                return BadRequest(new {message = "use adventure specific request to get source for your key"});
            }

            if (filters.Language == null)
            {
                return BadRequest(new {message = "language required"});
            }
            
            var source = await _sourcesService.GetSourceForKey(filters.Key.GetValueOrDefault(), Guid.Empty, filters.Language);
            return Ok(source);
        }
        
        [HttpGet("adventure/{adventureId:guid}/unreferenced")]
        [Authorize]
        public async Task<IActionResult> GetUnreferencedSourcesForAdventure(Guid adventureId)
        {
            var canAccessAdventure = await _permissionService.CanWriteAdventure(
                GetUserId().GetValueOrDefault(),
                adventureId);
            if(!canAccessAdventure)
            {
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            }

            var sources = await _sourcesService.GetUnreferencedSourcesForAdventure(adventureId);
            return Ok(sources);
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

            if (filters.Key != null && filters.Language != null)
            {
                var source = await _sourcesService.GetSourceForKey(filters.Key.GetValueOrDefault(), adventureId, filters.Language);
                return Ok(source);
            }
            if (filters.Language != null)
            {
                var sources = await _sourcesService.GetSourcesForAdventure(adventureId, filters.Language);
                return Ok(sources);
            }
            else
            {
                var sources = await _sourcesService.GetSourceAllLanguagesForAdventure(adventureId);
                return Ok(sources);
            }
            
        }
        
        [HttpGet("adventure/{adventureId:guid}/processed")]
        [Authorize]
        public async Task<IActionResult> GetProcessedSourcesForAdventure(Guid adventureId, [FromQuery]SourceFilterRequest filters)
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
            
            var source = await _sourcesService.GetProcessedSourceForKey(filters.Key.GetValueOrDefault(), adventureId, filters.Language);
            return Ok(source);
        }
        
        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateSource([FromBody] SourceUpdateRequest sourceUpdateRequest)
        {
            var canAccessAdventure = await _permissionService.CanReadAdventure(
                GetUserId().GetValueOrDefault(),
                sourceUpdateRequest.Source.AdventureId);
            if (!canAccessAdventure)
            {
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            }

            try
            {
                await _sourcesService.UpdateSource(sourceUpdateRequest);
                return Ok(null);
            }
            catch (Exception ex)
            {
                return BadRequest((new {message = ex.Message}));
            }
        }
    }
}