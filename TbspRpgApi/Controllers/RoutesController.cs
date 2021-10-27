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
    public class RoutesController : BaseController
    {
        private readonly IRoutesService _routesService;
        private ILogger<RoutesController> _logger;

        public RoutesController(IRoutesService routesService,
            ILogger<RoutesController> logger)
        {
            _routesService = routesService;
            _logger = logger;
        }

        [HttpGet("{routeId:guid}"), Authorize]
        public async Task<IActionResult> GetRouteById(Guid routeId)
        {
            var route = await _routesService.GetRouteById(routeId);

            if (route != null && !CanAccessLocation(route.LocationId))
            {
                return BadRequest(new { message = NotYourRouteErrorMessage });
            }

            return Ok(route);
        }
        
        [HttpGet("location/{locationId:guid}"), Authorize]
        public async Task<IActionResult> GetRoutesForLocation(Guid locationId)
        {
            if (!CanAccessLocation(locationId))
            {
                return BadRequest(new { message = NotYourRouteErrorMessage });
            }
                
            var routes = await _routesService.GetRoutes(new RouteFilterRequest()
            {
                LocationId = locationId
            });
            return Ok(routes);
        }
        
        [HttpGet("destination/{locationId:guid}"), Authorize]
        public async Task<IActionResult> GetRoutesWithDestination(Guid locationId)
        {
            if (!CanAccessLocation(locationId))
            {
                return BadRequest(new { message = NotYourRouteErrorMessage });
            }
                
            var routes = await _routesService.GetRoutes(new RouteFilterRequest()
            {
                DestinationLocationId = locationId
            });
            return Ok(routes);
        }
    }
}