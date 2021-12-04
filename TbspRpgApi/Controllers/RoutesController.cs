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
    public class RoutesController : BaseController
    {
        private readonly IRoutesService _routesService;
        private ILogger<RoutesController> _logger;

        public RoutesController(IRoutesService routesService,
            IUsersService usersService,
            ILogger<RoutesController> logger): base(usersService)
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
        
        [HttpPut, Authorize]
        public async Task<IActionResult> UpdateRoutesWithSource([FromBody] RouteUpdateRequest[] updateRouteRequests)
        {
            if (updateRouteRequests.Length == 0)
                return Ok(null);
            
            if (!CanAccessAdventure(updateRouteRequests[0].source.AdventureId))
            {
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            }

            try
            {
                await _routesService.UpdateRoutesWithSource(updateRouteRequests);
                return Ok(null);
            }
            catch (Exception ex)
            {
                return BadRequest((new {message = ex.Message}));
            }
        }
    }
}