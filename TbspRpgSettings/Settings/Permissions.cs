using System.Collections.Generic;

namespace TbspRpgSettings.Settings
{
    public class Permissions
    {
        public static readonly string READ_LOCATION = "read_location";
        public static readonly string WRITE_LOCATION = "write_location";

        public static List<string> GetAllPermissionNames()
        {
            return new List<string>()
            {
                READ_LOCATION,
                WRITE_LOCATION
            };
        }
    }
}