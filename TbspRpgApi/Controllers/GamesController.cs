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
    public class GamesController : BaseController
    {
        private readonly IGamesService _gamesService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(IGamesService gamesService,
            ILogger<GamesController> logger)
        {
            _gamesService = gamesService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(new [] {"admin"})]
        public async Task<IActionResult> GetGames([FromQuery] GameFilterRequest gameFilterRequest)
        {
            // in the future we'll relax the admin requirement
            // if the user isn't an admin well return a game view model that doesn't contain user information
            var games = await _gamesService.GetGames(gameFilterRequest);
            return Ok(games);
        }

        [HttpGet("adventure/{adventureId:guid}")]
        [Authorize]
        public async Task<IActionResult> GetGameByAdventure(Guid adventureId)
        {
            var userId = GetUserId();
            if (userId == null) return BadRequest(new {message = "couldn't get game by adventure id"});
            var game = await _gamesService.GetGameByAdventureIdAndUserId(adventureId, userId.GetValueOrDefault());
            return Ok(game);
        }
        
        [HttpGet("start/{adventureId:guid}")]
        [Authorize]
        public async Task<IActionResult> StartGame(Guid adventureId) {
            try
            {
                var userId = GetUserId();
                if (userId != null)
                {
                    await _gamesService.StartGame(userId.GetValueOrDefault(), adventureId, DateTime.UtcNow);
                }
                else
                {
                    return BadRequest(new { message = "couldn't start game" });
                }
            }
            catch
            {
                return BadRequest(new { message = "couldn't start game" });
            }

            return Accepted();
        }
    }
}