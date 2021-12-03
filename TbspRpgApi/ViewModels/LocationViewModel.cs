using System;
using TbspRpgApi.Entities;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels
{
    public class LocationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Initial { get; set; }
        public Guid SourceKey { get; set; }
        public Guid AdventureId { get; set; }

        public LocationViewModel() {}

        public LocationViewModel(Location location)
        {
            Id = location.Id;
            Name = location.Name;
            Initial = location.Initial;
            SourceKey = location.SourceKey;
            AdventureId = location.AdventureId;
        }

        public Location ToEntity()
        {
            return new Location()
            {
                Id = Id,
                AdventureId = AdventureId,
                Initial = Initial,
                Name = Name,
                SourceKey = SourceKey
            };
        }
    }
}