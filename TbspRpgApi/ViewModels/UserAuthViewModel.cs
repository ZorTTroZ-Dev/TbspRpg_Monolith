using TbspRpgApi.Entities;
using TbspRpgApi.JwtAuthorization;

namespace TbspRpgApi.ViewModels
{
    public class UserAuthViewModel : UserViewModel
    {
        public string Token { get; set; }
        
        public UserAuthViewModel(UserViewModel userViewModel)
        {
            Id = userViewModel.Id;
            Username = userViewModel.Username;
        }
    }
}