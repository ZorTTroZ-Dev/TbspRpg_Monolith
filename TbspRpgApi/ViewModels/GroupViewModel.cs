using System;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels
{
    public class GroupViewModel
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        public GroupViewModel(Group group)
        {
            Id = group.Id;
            Name = group.Name;
        }
    }
}