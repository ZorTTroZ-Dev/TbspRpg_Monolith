namespace TbspRpgSettings.Settings
{
    public interface ISmtpSettings
    {
        string Server { get; set; }
        int Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        bool SendMail { get; set; }
    }
    
    public class SmtpSettings: ISmtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool SendMail { get; set; }
    }
}