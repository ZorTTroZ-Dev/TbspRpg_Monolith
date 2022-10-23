using TbspRpgDataLayer.Entities;

namespace TbspRpgProcessor.Entities;

public class LocationUpdateModel
{
    public Location Location { get; set; }
    public Source Source { get; set; }
    public string Language { get; set; }
}