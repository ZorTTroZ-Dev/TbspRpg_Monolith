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
    public class AdventuresController: BaseController
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
        
        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateAdventureAndSource([FromBody] AdventureUpdateRequest adventureUpdateRequest)
        {
            if (adventureUpdateRequest.adventure.Id != Guid.Empty &&
                !CanAccessAdventure(adventureUpdateRequest.adventure.Id))
            {
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            }

            try
            {
                await _adventuresService.UpdateAdventureAndSource(adventureUpdateRequest, GetUserId().GetValueOrDefault());
                return Ok(null);
            }
            catch (Exception ex)
            {
                return BadRequest((new {message = ex.Message}));
            }
        }
    }
}