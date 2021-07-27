namespace TbspRpgSettings.Settings
{
    public interface IDatabaseSettings {
        string Username { get; set; }
        string Password { get; set; }
        string Name { get; set; }
        string Url { get; set; }
        string SystemDatabaseUrl { get; set; }
        string SystemDatabaseName { get; set; }
        string Salt { get; set; }
    }
    
    public class DatabaseSettings : IDatabaseSettings
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string SystemDatabaseUrl { get; set; }
        public string SystemDatabaseName { get; set; }
        public string Salt { get; set; }
    }
}