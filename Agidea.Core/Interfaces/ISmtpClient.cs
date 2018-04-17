using System.Net.Mail;
using System.Threading.Tasks;

namespace Agidea.Core.Interfaces
{
    public interface ISmtpClient
    {
        void Send(MailMessage mailMessage);

        Task SendMailAsync(MailMessage mailMessage);
    }
}