using System.Threading.Tasks;

namespace TbspRpgProcessor
{
    public interface IMailClient
    {
        Task SendRegistrationVerificationMail(string email, string registrationKey);
    }
    
    public class MailClient: IMailClient
    {
        public async Task SendRegistrationVerificationMail(string email, string registrationKey)
        {
            throw new System.NotImplementedException();
        }
    }
}