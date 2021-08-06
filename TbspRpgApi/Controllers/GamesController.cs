using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.Services;

namespace TbspRpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly IGamesService _gamesService;
        private readonly ILogger<GamesController> _logger;

        public GamesController(IGamesService gamesService,
            ILogger<GamesController> logger)
        {
            _gamesService = gamesService;
            _logger = logger;
        }
        
        [Route("start/{adventureId:guid}")]
        [Authorize]
        public async Task<IActionResult> Start(Guid adventureId) {
            try
            {
                if (HttpContext.Items[AuthorizeAttribute.USER_ID_CONTEXT_KEY] != null)
                {
                    var userId = (Guid) HttpContext.Items[AuthorizeAttribute.USER_ID_CONTEXT_KEY];
                    await _gamesService.StartGame(userId, adventureId, DateTime.Now);
                }
                else
                {
                    return BadRequest(new { message = "couldn't start game" });
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { message = "couldn't start game" });
            }

            return Accepted();
        }
    }
}