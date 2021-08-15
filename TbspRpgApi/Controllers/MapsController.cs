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
        private readonly ILogger<MapsController> _logger;

        public MapsController(IMapsService mapsService, ILogger<MapsController> logger)
        {
            _mapsService = mapsService;
            _logger = logger;
        }
        
        [HttpGet("location"), Authorize]
        public async Task<IActionResult> GetCurrentLocationForGame(Guid gameId) {
            if(!CanAccessGame(gameId))
                return BadRequest(new { message = "not your game" });
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
        
        [HttpGet("routes"), Authorize]
        public async Task<IActionResult> GetCurrentRoutesForGame(Guid gameId) {
            if(!CanAccessGame(gameId))
                return BadRequest(new { message = "not your game" });
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