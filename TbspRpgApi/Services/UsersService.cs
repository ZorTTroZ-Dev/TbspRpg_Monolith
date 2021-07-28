using System.Threading.Tasks;
using TbspRpgApi.ViewModels;

namespace TbspRpgApi.Services
{
    public interface IUsersService
    {
        Task<UserViewModel> Authenticate(string userName, string password);
    }
    
    public class UsersService : IUsersService
    {
        private readonly TbspRpgDataLayer.Services.IUsersService _usersService;

        public UsersService(TbspRpgDataLayer.Services.IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<UserViewModel> Authenticate(string userName, string password)
        {
            var user = await _usersService.Authenticate(userName, password);
            return user == null ? null : new UserViewModel(user);
        }
    }
}