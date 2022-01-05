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
        
        public string Email { get; set; }
        
        public bool RegistrationComplete { get; set; }
        
        public List<GroupViewModel> Groups { get; set; }
        
        public List<string> Permissions { get; set; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            Email = user.Email;
            RegistrationComplete = user.RegistrationComplete;
            if (user.Groups != null)
            {
                Groups = new List<GroupViewModel>();
                Permissions = new List<string>();
                foreach (var group in user.Groups)
                {
                  Groups.Add(new GroupViewModel(group));
                  foreach (var permission in group.Permissions)
                  {
                      Permissions.Add(permission.Name);
                  }
                }
            }
        }
    }
}