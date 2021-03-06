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
        public bool Final { get; set; }
        public Guid SourceKey { get; set; }
        public Guid AdventureId { get; set; }
        public Guid? EnterScriptId { get; set; }
        public Guid? ExitScriptId { get; set; }

        public LocationViewModel() {}

        public LocationViewModel(Location location)
        {
            Id = location.Id;
            Name = location.Name;
            Initial = location.Initial;
            Final = location.Final;
            SourceKey = location.SourceKey;
            AdventureId = location.AdventureId;
            EnterScriptId = location.EnterScriptId;
            ExitScriptId = location.ExitScriptId;
        }

        public Location ToEntity()
        {
            return new Location()
            {
                Id = Id,
                AdventureId = AdventureId,
                Initial = Initial,
                Final = Final,
                Name = Name,
                SourceKey = SourceKey,
                EnterScriptId = EnterScriptId,
                ExitScriptId = ExitScriptId
            };
        }
    }
}