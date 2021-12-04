using System;
using System.Collections.Generic;
using System.Linq;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        
        public string Username { get; set; }
        
        public List<GroupViewModel> Groups { get; set; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Username = user.UserName;
            if (user.Groups != null)
            {
                Groups = user.Groups.Select(group => new GroupViewModel(group)).ToList();
            }
        }
    }
}