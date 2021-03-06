using System.Collections.Generic;

namespace TbspRpgSettings.Settings
{
    public class Languages
    {
        public static readonly string ENGLISH = "en";
        public static readonly string SPANISH = "esp";
        public static readonly string DEFAULT = "en";

        public static List<string> GetAllLanguages()
        {
            return new List<string>()
            {
                ENGLISH,
                SPANISH
            };
        }
    }
}