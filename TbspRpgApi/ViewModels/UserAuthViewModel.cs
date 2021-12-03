using TbspRpgApi.Entities;
using TbspRpgApi.JwtAuthorization;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels
{
    public class UserAuthViewModel : UserViewModel
    {
        public string Token { get; }
        
        public UserAuthViewModel(User user, string token) : base(user)
        {
            Token = token;
        }
    }
}