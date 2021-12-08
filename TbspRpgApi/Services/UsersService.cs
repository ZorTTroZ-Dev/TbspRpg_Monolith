using System;
using System.Threading.Tasks;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.ViewModels;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.Services
{
    public interface IUsersService
    {
        Task<UserViewModel> Authenticate(string userName, string password);
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

        public async Task<UserViewModel> Authenticate(string userName, string password)
        {
            var user = await _usersService.Authenticate(userName, password);
            if (user == null) return null;
            var token = _jwtHelper.GenerateToken(user.Id.ToString());
            return new UserAuthViewModel(user, token);
        }
    }
}