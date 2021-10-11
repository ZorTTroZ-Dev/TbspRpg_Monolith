using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class RouteViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public Guid SourceKey { get; }
        public long TimeStamp { get; }

        public RouteViewModel(Route route, Game game)
        {
            Id = route.Id;
            Name = route.Name;
            SourceKey = route.SourceKey;
            TimeStamp = game.LocationUpdateTimeStamp;
        }

        public RouteViewModel(Route route)
        {
            Id = route.Id;
            Name = route.Name;
            SourceKey = route.SourceKey;
        }
    }
}