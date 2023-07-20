using System;
using System.Collections.Generic;
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
        public List<ObjectViewModel> AdventureObjects { get; set; }

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
            if (location.AdventureObjects != null)
            {
                AdventureObjects = new List<ObjectViewModel>();
                foreach (var obj in location.AdventureObjects)
                {
                    AdventureObjects.Add(new ObjectViewModel(obj));
                }
            }
        }

        public Location ToEntity()
        {
            var objectEntities = new List<AdventureObject>();
            foreach (var obj in AdventureObjects)
            {
                objectEntities.Add(obj.ToEntity());
            }
            return new Location()
            {
                Id = Id,
                AdventureId = AdventureId,
                Initial = Initial,
                Final = Final,
                Name = Name,
                SourceKey = SourceKey,
                EnterScriptId = EnterScriptId,
                ExitScriptId = ExitScriptId,
                AdventureObjects = objectEntities
            };
        }
    }
}