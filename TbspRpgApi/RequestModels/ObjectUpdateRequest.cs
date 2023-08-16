using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels;

public class ObjectUpdateRequest
{
    public ObjectViewModel adventureObject { get; set; }
    public SourceViewModel nameSource { get; set; }
    public SourceViewModel descriptionSource { get; set; }
    
    public AdventureObjectUpdateModel ToAdventureObjectUpdateModel()
    {
        return new AdventureObjectUpdateModel()
        {
            AdventureObject = adventureObject.ToEntity(),
            NameSource = nameSource.ToEntity(),
            DescriptionSource = descriptionSource.ToEntity(),
            Language = nameSource.Language
        };
    }
}