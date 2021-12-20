using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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
        public async Task<IActionResult> Register(UsersRegisterRequest registerRequest)
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

        [HttpGet("register/verify/{registrationKey}")]
        public async Task<IActionResult> RegisterVerify(string registrationKey)
        {
            try
            {
                var user = await _usersService.VerifyRegistration(registrationKey);
                return Ok(user);
            }
            catch
            {
                return BadRequest(new {message = "could not verify registration"});
            }
        }
        
        // registration process
        // on receive register request
        // check if there is already a user with that email address, if so bad request
        // insert user in to the database table populate a row called registration key
        // the registration key is the hash of the email and password
        // Send an email that instructs the user to click a link to register/verify/{regkey}
        // Until user clicks on email link the site will let them login but when they try to go somewhere show complete
        //  registration page
        // on request to register/verify/{regkey} find the user with that registration key and remove the key from
        //  their user row to complete registration
        // have a process that purges any users that have have a regkey and were created more than a week ago
    }
}