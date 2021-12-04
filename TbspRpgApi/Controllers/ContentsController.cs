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
    [Route("api/[controller]/{gameId:guid}")]
    public class ContentsController : BaseController
    {
        private readonly IContentsService _contentsService;
        private readonly IPermissionService _permissionService;
        private readonly ILogger<ContentsController> _logger;

        public ContentsController(IContentsService contentsService,
            IPermissionService permissionService,
            ILogger<ContentsController> logger)
        {
            _contentsService = contentsService;
            _permissionService = permissionService;
            _logger = logger;
        }
        
        [HttpGet("latest")]
        [Authorize]
        public async Task<IActionResult> GetLatestContentForGame(Guid gameId)
        {
            var canAccessGame = await _permissionService.CanAccessGame(
                GetUserId().GetValueOrDefault(),
                gameId);
            if(!canAccessGame)
                return BadRequest(new { message = NotYourGameErrorMessage });
            var contentViewModel = await _contentsService.GetLatestForGame(gameId);
            return Ok(contentViewModel);
        }
        
        [Authorize, HttpGet("filter")]
        public async Task<IActionResult> GetPartialContentForGame(Guid gameId, [FromQuery] ContentFilterRequest filterRequest) {
            var canAccessGame = await _permissionService.CanAccessGame(
                GetUserId().GetValueOrDefault(),
                gameId);
            if(!canAccessGame)
                return BadRequest(new { message = NotYourGameErrorMessage });
            try
            {
                var contentViewModel = await _contentsService.GetPartialContentForGame(gameId, filterRequest);
                return Ok(contentViewModel);
            }
            catch
            {
                return BadRequest(new { message = "invalid filter request" });
            }
        }

        [Authorize, HttpGet("after/{position}")]
        public async Task<IActionResult> GetContentForGameAfterPosition(Guid gameId, ulong position)
        {
            var canAccessGame = await _permissionService.CanAccessGame(
                GetUserId().GetValueOrDefault(),
                gameId);
            if(!canAccessGame)
                return BadRequest(new { message = NotYourGameErrorMessage });
            var contentViewModel = await _contentsService.GetContentForGameAfterPosition(gameId, position);
            return Ok(contentViewModel);
        }
        
        [Authorize, HttpGet("source/{key:guid}")]
        public async Task<IActionResult> GetSourceForKey(Guid gameId, Guid key) {
            var canAccessGame = await _permissionService.CanAccessGame(
                GetUserId().GetValueOrDefault(),
                gameId);
            if(!canAccessGame)
                return BadRequest(new { message = NotYourGameErrorMessage });
            // TODO: Make sure the key is part of the game content
            try
            {
                var source = await _contentsService.GetSourceForKey(gameId, key);
                if(source == null)
                    return BadRequest(new { message = "invalid source key request" });
                return Ok(source);
            }
            catch
            {
                return BadRequest(new { message = "invalid source key request" });
            }
        }
    }
}