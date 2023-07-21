using System;
using System.Collections.Generic;
using TbspRpgDataLayer.Entities;

namespace TbspRpgApi.ViewModels;

public class ObjectViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Type { get; set; }
    public Guid AdventureId { get; set; }
    public List<LocationViewModel> Locations { get; set; }

    public ObjectViewModel() { }

    public ObjectViewModel(AdventureObject adventureObject)
    {
        Id = adventureObject.Id;
        Name = adventureObject.Name;
        Description = adventureObject.Description;
        Type = adventureObject.Type;
        AdventureId = adventureObject.AdventureId;
        if (adventureObject.Locations != null)
        {
            Locations = new List<LocationViewModel>();
            foreach (var location in adventureObject.Locations)
            {
                Locations.Add(new LocationViewModel(location));
            }
        }
    }

    public AdventureObject ToEntity()
    {
        var locationEntities = new List<Location>();
        foreach (var locationViewModel in Locations)
        {
            locationEntities.Add(locationViewModel.ToEntity());
        }
        return new AdventureObject()
        {
            Id = Id,
            Name = Name,
            Description = Description,
            Type = Type,
            AdventureId = AdventureId,
            Locations = locationEntities
        };
    }
}