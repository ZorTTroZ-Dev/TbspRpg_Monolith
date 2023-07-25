using System.Text.RegularExpressions;
using TbspRpgSettings.Settings;

namespace TbspRpgSettings;

public class TbspRpgUtilities
{
    public Regex EmbeddedSourceScriptRegex = new Regex(@"<\s*" + SourceTagTypes.Script + @"\s*\:([^>]*)>");
}