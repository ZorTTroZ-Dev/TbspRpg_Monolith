using TbspRpgDataLayer.Entities;

namespace TbspRpgProcessor.Entities;

public class AdventureObjectUpdateModel
{
    public AdventureObject AdventureObject { get; set; }
    public Source NameSource { get; set; }
    public Source DescriptionSource { get; set; }
    public string Language { get; set; }
}