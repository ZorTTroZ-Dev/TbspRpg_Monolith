using System;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels
{
    public class UsersRegisterResendRequest
    {
        public Guid UserId { get; set; }
        
        public UserRegisterResendModel ToUserRegisterResendModel()
        {
            return new UserRegisterResendModel()
            {
                UserId = UserId
            };
        }
    }
}