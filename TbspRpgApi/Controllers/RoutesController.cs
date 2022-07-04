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
        private readonly IPermissionService _permissionService;
        private ILogger<RoutesController> _logger;

        public RoutesController(IRoutesService routesService,
            IPermissionService permissionService,
            ILogger<RoutesController> logger)
        {
            _routesService = routesService;
            _permissionService = permissionService;
            _logger = logger;
        }

        [HttpGet("{routeId:guid}"), Authorize]
        public async Task<IActionResult> GetRouteById(Guid routeId)
        {
            var route = await _routesService.GetRouteById(routeId);
            if (route != null)
            {
                var canAccessLocation = await _permissionService.CanReadLocation(
                    GetUserId().GetValueOrDefault(),
                    route.LocationId);
                if(!canAccessLocation)
                    return BadRequest(new { message = NotYourRouteErrorMessage });
            }

            return Ok(route);
        }
        
        [HttpGet("location/{locationId:guid}"), Authorize]
        public async Task<IActionResult> GetRoutesForLocation(Guid locationId)
        {
            var canAccessLocation = await _permissionService.CanReadLocation(
                GetUserId().GetValueOrDefault(),
                locationId);
            if (!canAccessLocation)
            {
                return BadRequest(new { message = NotYourRouteErrorMessage });
            }
                
            var routes = await _routesService.GetRoutes(new RouteFilterRequest()
            {
                LocationId = locationId
            });
            return Ok(routes);
        }
        
        [HttpGet("adventure/{adventureId:guid}"), Authorize]
        public async Task<IActionResult> GetRoutesForAdventure(Guid adventureId)
        {
            var canAccessAdventure = await _permissionService.CanReadAdventure(
                GetUserId().GetValueOrDefault(),
                adventureId);
            
            if (!canAccessAdventure)
            {
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            }
                
            var routes = await _routesService.GetRoutes(new RouteFilterRequest()
            {
                AdventureId = adventureId
            });
            return Ok(routes);
        }
        
        [HttpGet("destination/{locationId:guid}"), Authorize]
        public async Task<IActionResult> GetRoutesWithDestination(Guid locationId)
        {
            var canAccessLocation = await _permissionService.CanReadLocation(
                GetUserId().GetValueOrDefault(),
                locationId);
            if (!canAccessLocation)
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
            
            var canAccessAdventure = await _permissionService.CanWriteLocation(
                GetUserId().GetValueOrDefault(),
                updateRouteRequests[0].route.LocationId);
            if (!canAccessAdventure)
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
        
        [HttpDelete("{routeId:guid}"), Authorize]
        public async Task<IActionResult> DeleteRoute(Guid routeId)
        {
            var canDeleteRoute = await _permissionService.CanDeleteRoute(GetUserId().GetValueOrDefault(), routeId);
            if(!canDeleteRoute)
                return BadRequest(new { message = NotYourAdventureErrorMessage });
            
            try
            {
                await _routesService.DeleteRoute(routeId);
                return Ok();
            }
            catch
            {
                return BadRequest(new { message = "couldn't delete route" });
            }
        }
    }
}