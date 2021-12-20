using System;

namespace TbspRpgProcessor.Entities
{
    public class UserVerifyRegisterModel
    {
        public Guid UserId { get; set; }
        public string RegistrationKey { get; set; }
    }
}