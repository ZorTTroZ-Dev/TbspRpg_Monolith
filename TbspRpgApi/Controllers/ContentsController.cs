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
        private readonly ILogger<ContentsController> _logger;

        public ContentsController(IContentsService contentsService,
            ILogger<ContentsController> logger)
        {
            _contentsService = contentsService;
            _logger = logger;
        }
        
        [Route("latest")]
        [Authorize]
        public async Task<IActionResult> GetLatestContentForGame(Guid gameId)
        {
            var contentViewModel = await _contentsService.GetLatestForGame(gameId);
            return Ok(contentViewModel);
        }
        
        [Authorize, HttpGet("filter")]
        public async Task<IActionResult> GetPartialContentForGame(Guid gameId, [FromQuery] ContentFilterRequest filterRequest) {
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
            var contentViewModel = await _contentsService.GetContentForGameAfterPosition(gameId, position);
            return Ok(contentViewModel);
        }
    }
}