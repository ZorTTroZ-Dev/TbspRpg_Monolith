using System.Collections.Generic;

namespace TbspRpgSettings.Settings
{
    public class Permissions
    {
        public const string ReadLocation = "read_location";
        public const string WriteLocation = "write_location";
        public const string ReadAdventure = "read_adventure";
        public const string WriteAdventure = "write_adventure";
        public const string ReadGame = "read_game";
        public const string WriteGame = "write_game";
        public const string AdminGroup = "admin";

        public static List<string> GetAllPermissionNames()
        {
            return new List<string>()
            {
                ReadLocation,
                WriteLocation,
                ReadAdventure,
                WriteAdventure,
                ReadGame,
                WriteGame
            };
        }
    }
}