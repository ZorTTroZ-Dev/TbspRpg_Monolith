using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.RequestModels;
using TbspRpgApi.Services;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> _logger;
        
        public UsersController(IUsersService usersService,
            ILogger<UsersController> logger): base(usersService)
        {
            _usersService = usersService;
            _logger = logger;
        }
        
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UsersAuthenticateRequest model)
        {
            var userViewModel = await _usersService.Authenticate(
                model.UserName, model.Password);

            if (userViewModel != null)
            {
                return Ok(userViewModel);
            }
            
            _logger.LogDebug("authentication failed for {Username}", model.UserName);
            return BadRequest(new {message = "Username or password is incorrect"});
        }
    }
}