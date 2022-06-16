using System;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels
{
    public class RouteViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SourceKey { get; set; }
        public Guid RouteTakenSourceKey { get; set; }
        public Guid LocationId { get; set; }
        public Guid DestinationLocationId { get; set; }
        public long TimeStamp { get; }
        public Guid? RouteTakenScriptId { get; set; }

        public RouteViewModel() { }

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
            RouteTakenSourceKey = route.RouteTakenSourceKey;
            LocationId = route.LocationId;
            DestinationLocationId = route.DestinationLocationId;
            RouteTakenScriptId = route.RouteTakenScriptId;
        }

        public Route ToEntity()
        {
            return new Route()
            {
                Id = Id,
                Name = Name,
                SourceKey = SourceKey,
                RouteTakenSourceKey = RouteTakenSourceKey,
                LocationId = LocationId,
                DestinationLocationId = DestinationLocationId,
                RouteTakenScriptId = RouteTakenScriptId
            };
        }
    }
}