using System;
using System.Collections.Generic;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels;

public class ObjectViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public Guid AdventureId { get; set; }

    public ObjectViewModel() { }

    public ObjectViewModel(AdventureObject adventureObject)
    {
        Id = adventureObject.Id;
        Name = adventureObject.Name;
        Type = adventureObject.Type;
        AdventureId = adventureObject.AdventureId;
    }

    public AdventureObject ToEntity()
    {
        return new AdventureObject()
        {
            Id = Id,
            Name = Name,
            Type = Type,
            AdventureId = AdventureId
        };
    }
}