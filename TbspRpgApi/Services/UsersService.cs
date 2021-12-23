using System;
using System.Threading.Tasks;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;
using TbspRpgProcessor.Processors;

namespace TbspRpgApi.Services
{
    public interface IUsersService
    {
        Task<UserViewModel> Authenticate(string email, string password);
        Task<UserViewModel> Register(UsersRegisterRequest registerRequest);
        Task<UserViewModel> VerifyRegistration(UsersRegisterVerifyRequest verifyRequest);
    }
    
    public class UsersService : IUsersService
    {
        private readonly TbspRpgDataLayer.Services.IUsersService _usersService;
        private readonly IUserProcessor _userProcessor;
        private readonly IJwtHelper _jwtHelper;

        public UsersService(
            TbspRpgDataLayer.Services.IUsersService usersService,
            IUserProcessor userProcessor,
            IJwtSettings jwtSettings)
        {
            _usersService = usersService;
            _userProcessor = userProcessor;
            _jwtHelper = new JwtHelper(jwtSettings.Secret);
        }

        public async Task<UserViewModel> Authenticate(string email, string password)
        {
            var user = await _usersService.Authenticate(email, password);
            if (user == null) return null;
            var token = _jwtHelper.GenerateToken(user.Id.ToString());
            return new UserAuthViewModel(user, token);
        }

        public async Task<UserViewModel> Register(UsersRegisterRequest registerRequest)
        {
            var user = await _userProcessor.RegisterUser(registerRequest.ToUserRegisterModel());
            return new UserViewModel(user);
        }

        public async Task<UserViewModel> VerifyRegistration(UsersRegisterVerifyRequest verifyRequest)
        {
            var user = await _userProcessor.VerifyUserRegistration(verifyRequest.ToUserVerifyRegisterModel());
            return user == null ? null : new UserViewModel(user);
        }
    }
}