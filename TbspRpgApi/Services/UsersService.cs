using System;
using System.Threading.Tasks;
using TbspRpgApi.JwtAuthorization;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services
{
    public interface IUsersService
    {
        Task<UserViewModel> Authenticate(string userName, string password);
        Task<UserViewModel> GetUserById(Guid userId);
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

        public async Task<UserViewModel> GetUserById(Guid userId)
        {
            var user = await _usersService.GetById(userId);
            return new UserViewModel(user);
        }
    }
}