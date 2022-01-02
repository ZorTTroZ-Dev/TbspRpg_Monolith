using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TbspRpgApi.RequestModels;
using TbspRpgApi.Services;

namespace TbspRpgApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BaseController
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UsersController> _logger;
        
        public UsersController(IUsersService usersService,
            ILogger<UsersController> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }
        
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(UsersAuthenticateRequest model)
        {
            var userViewModel = await _usersService.Authenticate(
                model.Email, model.Password);

            if (userViewModel != null)
            {
                return Ok(userViewModel);
            }
            
            _logger.LogDebug("authentication failed for {Username}", model.Email);
            return BadRequest(new {message = "Username or password is incorrect"});
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UsersRegisterRequest registerRequest)
        {
            try
            {
                var user = await _usersService.Register(registerRequest);
                return Ok(user);
            }
            catch
            {
                return BadRequest(new { message = "registration failed" });
            }
        }
        
        [HttpPost("register/resend")]
        public async Task<IActionResult> RegisterResend([FromBody]UsersRegisterResendRequest registerRequest)
        {
            try
            {
                var user = await _usersService.RegisterResend(registerRequest);
                return Ok(user);
            }
            catch
            {
                return BadRequest(new { message = "registration resend failed" });
            }
        }

        [HttpPost("register/verify")]
        public async Task<IActionResult> RegisterVerify([FromBody]UsersRegisterVerifyRequest verifyRequest)
        {
            try
            {
                var user = await _usersService.VerifyRegistration(verifyRequest);
                return Ok(user);
            }
            catch
            {
                return BadRequest(new {message = "could not verify registration"});
            }
        }
    }
}