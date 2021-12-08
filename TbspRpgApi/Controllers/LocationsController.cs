﻿using System;
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
        private readonly IPermissionService _permissionService;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ILocationsService locationsService,
            IPermissionService permissionService,
            ILogger<LocationsController> logger)
        {
            _locationsService = locationsService;
            _permissionService = permissionService;
            _logger = logger;
        }
        
        [HttpGet("{adventureId:guid}"), Authorize]
        public async Task<IActionResult> GetLocationsForAdventure(Guid adventureId) {
            var canAccessAdventure = await _permissionService.CanReadAdventure(
                GetUserId().GetValueOrDefault(),
                adventureId);
            if(!canAccessAdventure)
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
        public async Task<IActionResult> UpdateLocationAndSource([FromBody] LocationUpdateRequest locationUpdateRequest)
        {
            var canAccessAdventure = await _permissionService.CanReadAdventure(
                GetUserId().GetValueOrDefault(),
                locationUpdateRequest.location.AdventureId);
            if (!canAccessAdventure)
            {
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            }

            try
            {
                await _locationsService.UpdateLocationAndSource(locationUpdateRequest);
                return Ok(null);
            }
            catch (Exception ex)
            {
                return BadRequest((new {message = ex.Message}));
            }
        }
    }
}