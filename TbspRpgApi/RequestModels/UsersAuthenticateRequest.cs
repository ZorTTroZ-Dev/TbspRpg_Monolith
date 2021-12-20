using System.ComponentModel.DataAnnotations;

namespace TbspRpgApi.RequestModels
{
    public class UsersAuthenticateRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}