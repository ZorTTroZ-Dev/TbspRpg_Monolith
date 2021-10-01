using System;
using TbspRpgApi.Entities;

namespace TbspRpgApi.ViewModels
{
    public class LocationViewModel
    {
        public Guid Id { get; }
        public string Name { get; }
        public bool Initial { get; }
        public Guid SourceKey { get; }
        public Guid AdventureId { get; }

        public LocationViewModel(Location location)
        {
            Id = location.Id;
            Name = location.Name;
            Initial = location.Initial;
            SourceKey = location.SourceKey;
            AdventureId = location.AdventureId;
        }
    }
}