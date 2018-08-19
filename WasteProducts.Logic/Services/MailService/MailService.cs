using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Services.MailService;

namespace WasteProducts.Logic.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly SmtpClient _smtpClient;

        public MailService(SmtpClient smtpClient, string ourEmail, IMailFactory mailFactory)
        {
            _smtpClient = smtpClient;
            OurEmail = ourEmail;
            MailFactory = mailFactory;
        }

        public IMailFactory MailFactory { get; }

        public string OurEmail { get; set; }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }

        public bool IsValidEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            await Task.Run(() =>
            {
                using (MailMessage message = MailFactory.Create(OurEmail, to, subject, body))
                {
                    _smtpClient.SendAsync(message, null);
                }
            });
        }
    }
}
