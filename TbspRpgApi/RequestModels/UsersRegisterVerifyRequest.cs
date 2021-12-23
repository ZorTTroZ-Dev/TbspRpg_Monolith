using System;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels
{
    public class UsersRegisterVerifyRequest
    {
        public Guid UserId { get; set; }
        public string RegistrationKey { get; set; }

        public UserVerifyRegisterModel ToUserVerifyRegisterModel()
        {
            return new UserVerifyRegisterModel()
            {
                UserId = UserId,
                RegistrationKey = RegistrationKey
            };
        }
    }
}