using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.Services;

namespace TbspRpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/{gameId:guid}")]
    public class MapsController : BaseController
    {
        private readonly IMapsService _mapsService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<MapsController> _logger;

        public MapsController(IMapsService mapsService,
            IPermissionService permissionService,
            ILogger<MapsController> logger)
        {
            _mapsService = mapsService;
            _permissionService = permissionService;
            _logger = logger;
        }
        
        [HttpGet("location"), Authorize]
        public async Task<IActionResult> GetCurrentLocationForGame(Guid gameId) {
            var canAccessGame = await _permissionService.CanReadGame(GetUserId().GetValueOrDefault(), gameId);
            if(!canAccessGame)
            {
                return BadRequest(new { message = NotYourGameErrorMessage });
            }

            try
            {
                var location = await _mapsService.GetCurrentLocationForGame(gameId);
                return Ok(location);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
        
        [HttpGet("changelocation/{routeId:guid}"), Authorize]
        public async Task<IActionResult> ChangeLocationViaRoute(Guid gameId, Guid routeId) {
            var canAccessGame = await _permissionService.CanReadGame(GetUserId().GetValueOrDefault(), gameId);
            if(!canAccessGame)
                return BadRequest(new { message = NotYourGameErrorMessage });
            try
            {
                return Ok(await _mapsService.ChangeLocationViaRoute(gameId, routeId, DateTime.UtcNow));
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
        
        [HttpGet("routes"), Authorize]
        public async Task<IActionResult> GetCurrentRoutesForGame(Guid gameId) {
            var canAccessGame = await _permissionService.CanReadGame(GetUserId().GetValueOrDefault(), gameId);
            if(!canAccessGame)
                return BadRequest(new { message = NotYourGameErrorMessage });
            try
            {
                var routes = await _mapsService.GetCurrentRoutesForGame(gameId);
                return Ok(routes);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }
    }
}