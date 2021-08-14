using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class RouteViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        
        //public string DisplayText { get; set; }

        public RouteViewModel(Route route)
        {
            Id = route.Id;
            Name = route.Name;
        }
    }
}