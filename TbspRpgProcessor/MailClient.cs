using System.Threading.Tasks;
using MailKit.Net.Smtp;
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
            // message.From.Add (new MailboxAddress ("Joey Tribbiani", "joey@friends.com"));
            message.To.Add (new MailboxAddress (email, email));
            message.Subject = "How you doin'?";

            message.Body = new TextPart ("plain") {
                Text = @"Hey Chandler,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?

-- Joey"
            };
            await SendMail(message);
        }

        private async Task SendMail(MimeMessage mail)
        {
            if (!_smtpSettings.SendMail)
                return;
            using var client = new SmtpClient ();
            await client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, false);
            await client.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
            await client.SendAsync(mail);
            await client.DisconnectAsync(true);
        }
    }
}