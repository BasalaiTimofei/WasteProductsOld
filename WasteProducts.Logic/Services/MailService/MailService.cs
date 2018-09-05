using System;
using System.Net.Mail;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Services.MailService;

namespace WasteProducts.Logic.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly SmtpClient _smtpClient;

        private bool _disposed;

        public MailService(SmtpClient smtpClient, string ourEmail, IMailFactory mailFactory)
        {
            _smtpClient = smtpClient;
            if (!IsValidEmail(ourEmail))
            {
                throw new FormatException("Arguement \"ourEmail\" in creating a new MailService inctance wasn't actually valid email.");
            }
            OurEmail = ourEmail;
            MailFactory = mailFactory;
        }

        public IMailFactory MailFactory { get; }

        public string OurEmail { get; set; }

        ~MailService()
        {
            if (!_disposed)
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _smtpClient?.Dispose();
                _disposed = true;
            }
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
            await Task.Run(async () =>
            {
                MailMessage message = null;
                SmtpClient smtpClient = null;
                try
                {
                    message = MailFactory.Create(OurEmail, to, subject, body);
                    await smtpClient.SendMailAsync(message);
                }
                catch
                {
                }
                finally
                {
                    message?.Dispose();
                    smtpClient?.Dispose();
                }
            });
        }
    }
}
