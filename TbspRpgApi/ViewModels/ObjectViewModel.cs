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
    public Guid NameSourceKey { get; set; }
    public Guid DescriptionSourceKey { get; set; }

    public ObjectViewModel() { }

    public ObjectViewModel(AdventureObject adventureObject)
    {
        Id = adventureObject.Id;
        Name = adventureObject.Name;
        Description = adventureObject.Description;
        Type = adventureObject.Type;
        AdventureId = adventureObject.AdventureId;
        NameSourceKey = adventureObject.NameSourceKey;
        DescriptionSourceKey = adventureObject.DescriptionSourceKey;
        Locations = new List<LocationViewModel>();
        if (adventureObject.Locations != null)
        {
            foreach (var location in adventureObject.Locations)
            {
                Locations.Add(new LocationViewModel(location, false));
            }
        }
    }

    public AdventureObject ToEntity()
    {
        var locationEntities = new List<Location>();
        if (Locations != null)
        {
            foreach (var locationViewModel in Locations)
            {
                locationEntities.Add(locationViewModel.ToEntity());
            }    
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