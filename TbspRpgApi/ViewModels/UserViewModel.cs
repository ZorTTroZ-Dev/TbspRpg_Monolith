using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; }
        
        public string Username { get; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Username = user.UserName;
        }
    }
}