using System;
using System.Net.Mail;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Services.Mail;

namespace WasteProducts.Logic.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly SmtpClient _smtpClient;

        private bool _disposed;

        public MailService(SmtpClient smtpClient, string ourEmail)
        {
            _smtpClient = smtpClient;
            if (!IsValidEmail(ourEmail))
            {
                throw new FormatException("Arguement \"ourEmail\" in creating a new MailService inctance wasn't actually valid email.");
            }
            OurEmail = ourEmail;
        }

        public string OurEmail { get; set; }

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
            MailMessage message = null;
            try
            {
                message = new MailMessage(OurEmail, to, subject, body);
                await _smtpClient.SendMailAsync(message);
            }
            finally
            {
                message?.Dispose();
            }
        }

        ~MailService()
        {
            Dispose();
        }
    }
}
