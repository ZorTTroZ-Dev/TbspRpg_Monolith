using System;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class RouteViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SourceKey { get; set; }
        public Guid SuccessSourceKey { get; set; }
        public Guid LocationId { get; set; }
        public Guid DestinationLocationId { get; set; }
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
            SuccessSourceKey = route.SuccessSourceKey;
            LocationId = route.LocationId;
            DestinationLocationId = route.DestinationLocationId;
        }

        public Route ToEntity()
        {
            return new Route()
            {
                Id = Id,
                Name = Name,
                SourceKey = SourceKey,
                SuccessSourceKey = SuccessSourceKey,
                LocationId = LocationId,
                DestinationLocationId = DestinationLocationId
            };
        }
    }
}