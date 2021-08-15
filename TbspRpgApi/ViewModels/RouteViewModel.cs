using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class RouteViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public Guid SourceKey { get; }

        public RouteViewModel(Route route)
        {
            Id = route.Id;
            Name = route.Name;
            SourceKey = route.SourceKey;
        }
    }
}