using TbspRpgDataLayer.Entities;

namespace TbspRpgProcessor.Entities;

public class SourceCreateOrUpdateModel
{
    public Source Source { get; set; }
    public string Language { get; set; }
    public bool Save { get; set; } = false;
}