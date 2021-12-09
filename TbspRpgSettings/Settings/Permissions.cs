using System.Collections.Generic;

namespace TbspRpgSettings.Settings
{
    public class Permissions
    {
        public static readonly string READ_LOCATION = "read_location";
        public static readonly string WRITE_LOCATION = "write_location";
        public static readonly string READ_ADVENTURE = "read_adventure";
        public static readonly string WRITE_ADVENTURE = "write_adventure";
        public static readonly string READ_GAME = "read_game";
        public static readonly string WRITE_GAME = "write_game";

        public static List<string> GetAllPermissionNames()
        {
            return new List<string>()
            {
                READ_LOCATION,
                WRITE_LOCATION,
                READ_ADVENTURE,
                WRITE_ADVENTURE
            };
        }
    }
}