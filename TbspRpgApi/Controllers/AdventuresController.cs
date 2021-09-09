using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.Services;

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
        public async Task<IActionResult> GetAllAdventures([FromQuery]AdventureFilterRequest filters)
        {
            var adventures = await _adventuresService.GetAllAdventures(filters);
            return Ok(adventures);
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAdventureById(Guid id)
        {
            var adventure = await _adventuresService.GetAdventureById(id);
            return Ok(adventure);
        }
        
        [HttpGet("{name}")]
        public async Task<IActionResult> GetAdventureByName(string name) {
            var adventure = await _adventuresService.GetAdventureByName(name);
            return Ok(adventure);
        }
    }
}