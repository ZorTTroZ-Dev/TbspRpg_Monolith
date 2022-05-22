using TbspRpgApi.ViewModels;
using TbspRpgProcessor.Entities;

namespace TbspRpgApi.RequestModels;

public class ScriptUpdateRequest
{
    public ScriptViewModel script { get; set; }

    public ScriptUpdateModel ToScriptUpdateModel()
    {
        return new ScriptUpdateModel()
        {
            script = script.ToEntity()
        };
    }
}