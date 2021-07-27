using Microsoft.AspNetCore.Mvc;

namespace TbspRpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // private readonly IUsersService _usersService;
        // private readonly ILogger<UsersController> _logger;
        //
        // public UsersController(IUsersService usersService, ILogger<UsersController> logger)
        // {
        //     _usersService = usersService;
        //     _logger = logger;
        // }
        //
        // [HttpPost("authenticate")]
        // public async Task<IActionResult> Authenticate(UsersAuthenticateRequest model)
        // {
        //     var user = await _usersService.GetUserByUserNameAndPassword(
        //         model.UserName, model.Password);
        //
        //     if (user == null)
        //     {
        //         _logger.LogDebug("authentication failed for {Username}", model.UserName);
        //         return BadRequest(new {message = "Username or password is incorrect"});
        //     }
        //
        //     return Ok(new UserViewModel(user));
        // }
    }
}