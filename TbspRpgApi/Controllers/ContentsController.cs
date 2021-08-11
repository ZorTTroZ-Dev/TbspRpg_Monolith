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
        
        [Route("{gameId:guid}/latest")]
        [Authorize]
        public async Task<IActionResult> GetLatestContentForGame(Guid gameId)
        {
            var contentViewModel = await _contentsService.GetLatestForGame(gameId);
            return Ok(contentViewModel);
        }
    }
}