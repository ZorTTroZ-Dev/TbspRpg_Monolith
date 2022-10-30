using System.Collections.Generic;
using TbspRpgDataLayer.Entities;

namespace TbspRpgProcessor.Entities;

public class LocationsRemoveModel
{
    public ICollection<Location> Locations { get; set; }
    public bool Save { get; set; } = true;
}