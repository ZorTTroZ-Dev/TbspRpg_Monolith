using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class LocationViewModel
    {
        public Guid Id { get; }
        public string Name { get; }

        public LocationViewModel(Location location)
        {
            Id = location.Id;
            Name = location.Name;
        }
    }
}