using System.Net.Mail;
using WasteProducts.Logic.Common.Services.MailService;

namespace WasteProducts.Logic.Services.MailService
{
    public class MailFactory : IMailFactory
    {
        public MailMessage Create(string from, string to, string subject, string body)
        {
            return new MailMessage(from, to, subject, body);
        }
    }
}
