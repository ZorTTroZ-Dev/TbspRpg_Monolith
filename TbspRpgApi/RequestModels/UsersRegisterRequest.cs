using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels
{
    public class UsersRegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public UserRegisterModel ToUserRegisterModel()
        {
            return new UserRegisterModel()
            {
                Email = Email,
                Password = Password
            };
        }
    }
}