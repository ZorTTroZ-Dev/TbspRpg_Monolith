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
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IJwtHelper _jwtHelper;
        private readonly ILogger<UsersController> _logger;
        
        public UsersController(IUsersService usersService, IJwtSettings jwtSettings, ILogger<UsersController> logger)
        {
            _usersService = usersService;
            _jwtHelper = new JwtHelper(jwtSettings.Secret);
            _logger = logger;
        }
        
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UsersAuthenticateRequest model)
        {
            var userViewModel = await _usersService.Authenticate(
                model.UserName, model.Password);

            if (userViewModel != null)
            {
                var token = _jwtHelper.GenerateToken(userViewModel.Id.ToString());
                var userAuthViewModel = new UserAuthViewModel(userViewModel);
                userAuthViewModel.Token = token;
                return Ok(userAuthViewModel);
            }
            
            _logger.LogDebug("authentication failed for {Username}", model.UserName);
            return BadRequest(new {message = "Username or password is incorrect"});
        }
    }
}