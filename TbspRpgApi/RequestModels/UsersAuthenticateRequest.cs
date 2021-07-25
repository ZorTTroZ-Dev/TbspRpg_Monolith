using System.ComponentModel.DataAnnotations;

namespace TbspRpgApi.RequestModels
{
    public class UsersAuthenticateRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}