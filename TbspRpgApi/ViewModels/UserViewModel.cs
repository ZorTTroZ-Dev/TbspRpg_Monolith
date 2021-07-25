using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; }
        
        public string UserName { get; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            UserName = user.UserName;
        }
    }
}