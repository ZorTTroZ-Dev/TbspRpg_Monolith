using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TbspRpgSettings.Settings;

namespace TbspRpgProcessor
{
    public interface IMailClient
    {
        Task SendRegistrationVerificationMail(string email, string registrationKey);
    }
    
    public class MailClient: IMailClient
    {
        private readonly ISmtpSettings _smtpSettings;

        public MailClient(ISmtpSettings smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }
        
        public async Task SendRegistrationVerificationMail(string email, string registrationKey)
        {
            var message = new MimeMessage ();
            message.To.Add(MailboxAddress.Parse(email));
            message.From.Add(MailboxAddress.Parse("no-reply@zorttroz.com"));
            message.Subject = "TbspRpg Registration Code";

            message.Body = new TextPart ("plain") {
                Text = $@"Your TbspRpg registration code is:

{registrationKey}

Have splendid adventures!"
            };
            await SendMail(message);
        }

        private async Task SendMail(MimeMessage mail)
        {
            if (!_smtpSettings.SendMail)
                return;
            using var client = new SmtpClient ();
            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port,  SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await client.SendAsync(mail);
            await client.DisconnectAsync(true);
        }
    }
}