using Microsoft.AspNetCore.Mvc;

namespace TbspRpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        // [HttpPost("authenticate")]
        // public async Task<IActionResult> Authenticate(AuthenticateRequest model)
        // {
        //     var response = await _userService.Authenticate(model);
        //
        //     if (response == null)
        //         return BadRequest(new { message = "Username or password is incorrect" });
        //
        //     return Ok(response);
        // }
    }
}