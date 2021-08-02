using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        
        public string Username { get; set; }
        
        public UserViewModel(User user)
        {
            Id = user.Id;
            Username = user.UserName;
        }

        public UserViewModel()
        {
            
        }
    }
}