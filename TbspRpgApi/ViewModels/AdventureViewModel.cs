using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class AdventureViewModel
    {
        public Guid Id { get; }
        public string Name { get; }

        public AdventureViewModel(Adventure adventure)
        {
            Id = adventure.Id;
            Name = adventure.Name;
        }
    }
}