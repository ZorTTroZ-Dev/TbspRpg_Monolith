using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TbspRpgDataLayer.Entities;
using TbspRpgDataLayer.Services;
using TbspRpgProcessor.Entities;

namespace TbspRpgProcessor.Processors
{
    public interface IUserProcessor
    {
        Task<User> RegisterUser(UserRegisterModel userRegisterModel);
        Task<User> VerifyUserRegistration(UserVerifyRegisterModel userVerifyRegisterModel);
    }
    
    public class UserProcessor : IUserProcessor
    {
        private readonly IUsersService _usersService;
        private readonly ILogger<UserProcessor> _logger;

        public UserProcessor(IUsersService usersService,
            ILogger<UserProcessor> logger)
        {
            _usersService = usersService;
            _logger = logger;
        }

        public async Task<User> RegisterUser(UserRegisterModel userRegisterModel)
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> VerifyUserRegistration(UserVerifyRegisterModel userVerifyRegisterModel)
        {
            throw new System.NotImplementedException();
        }
    }
}