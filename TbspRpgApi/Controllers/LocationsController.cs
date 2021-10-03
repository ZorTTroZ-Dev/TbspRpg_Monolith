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
    public class LocationsController: BaseController
    {
        private readonly ILocationsService _locationsService;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ILocationsService locationsService, ILogger<LocationsController> logger)
        {
            _locationsService = locationsService;
            _logger = logger;
        }
        
        [HttpGet("{adventureId:guid}"), Authorize]
        public async Task<IActionResult> GetLocationsForAdventure(Guid adventureId) {
            if(!CanAccessAdventure(adventureId))
            {
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            }

            try
            {
                var locations = await _locationsService.GetLocationsForAdventure(adventureId);
                return Ok(locations);
            }
            catch (Exception ex)
            {
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateLocationAndSource([FromBody] UpdateLocationRequest updateRequest)
        {
            if (!CanAccessAdventure(updateRequest.location.AdventureId))
            {
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            }

            try
            {
                await _locationsService.UpdateLocationAndSource(updateRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest((new {message = ex.Message}));
            }
        }
    }
}