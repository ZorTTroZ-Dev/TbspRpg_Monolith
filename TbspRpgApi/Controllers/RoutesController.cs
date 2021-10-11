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
    public class RoutesController : ControllerBase
    {
        private readonly IRoutesService _routesService;
        private ILogger<RoutesController> _logger;

        public RoutesController(IRoutesService routesService,
            ILogger<RoutesController> logger)
        {
            _routesService = routesService;
            _logger = logger;
        }
        
        [HttpGet, Authorize]
        public async Task<IActionResult> GetRoutes([FromQuery]RouteFilterRequest filters)
        {
            var routes = await _routesService.GetRoutes(filters);
            return Ok(routes);
        }
    }
}