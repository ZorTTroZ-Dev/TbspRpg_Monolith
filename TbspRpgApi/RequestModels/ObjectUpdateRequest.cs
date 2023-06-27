using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels;

public class ObjectUpdateRequest
{
    public ObjectViewModel obj { get; set; }
    
    public AdventureObjectUpdateModel ToAdventureObjectUpdateModel()
    {
        return new AdventureObjectUpdateModel()
        {
            adventureObject = obj.ToEntity()
        };
    }
}