using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.Services;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdventuresController : ControllerBase
    {
        private readonly IAdventuresService _adventuresService;
        private ILogger<AdventuresController> _logger;

        public AdventuresController(IAdventuresService adventuresService,
            ILogger<AdventuresController> logger)
        {
            _adventuresService = adventuresService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAllAdventures()
        {
            var adventures = await _adventuresService.GetAllAdventures();
            return Ok(adventures);
        }
    }
}