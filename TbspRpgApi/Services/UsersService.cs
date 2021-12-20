using System;
using System.Threading.Tasks;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.RequestModels;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.Services
{
    public interface IUsersService
    {
        Task<UserViewModel> Authenticate(string email, string password);
        Task<UserViewModel> Register(UsersRegisterRequest registerRequest);
        Task<UserViewModel> VerifyRegistration(string registrationKey);
    }
    
    public class UsersService : IUsersService
    {
        private readonly TbspRpgDataLayer.Services.IUsersService _usersService;
        private readonly IJwtHelper _jwtHelper;

        public UsersService(TbspRpgDataLayer.Services.IUsersService usersService, IJwtSettings jwtSettings)
        {
            _usersService = usersService;
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
            throw new NotImplementedException();
        }

        public async Task<UserViewModel> VerifyRegistration(string registrationKey)
        {
            throw new NotImplementedException();
        }
    }
}