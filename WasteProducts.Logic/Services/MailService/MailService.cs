using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using WasteProducts.Logic.Common.Services.MailService;

namespace WasteProducts.Logic.Services.MailService
{
    public class MailService : IMailService
    {
        private readonly SmtpClientGetter _smtpClientGetter;

        public MailService(SmtpClient smtpClient, string ourEmail, IMailFactory mailFactory)
        {
            _smtpClientGetter = new SmtpClientGetter(smtpClient);
            OurEmail = ourEmail;
            MailFactory = mailFactory;
        }

        public IMailFactory MailFactory { get; }

        public string OurEmail { get; set; }

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
                    smtpClient = _smtpClientGetter.Get();
                    await smtpClient.SendMailAsync(message);
                }
                finally
                {
                    message?.Dispose();
                    smtpClient?.Dispose();
                }
            });
        }

        private class SmtpClientGetter
        {
            private ICredentialsByHost Credentials { get; }
            private SmtpDeliveryFormat DeliveryFormat { get; }
            private SmtpDeliveryMethod DeliveryMethod { get; }
            private bool EnableSsl { get; }
            private string Host { get; }
            private string PickupDirectoryLocation { get; }
            private int Port { get; }
            private string TargetName { get; }
            private int Timeout { get; }
            private bool UseDefaultCredentials { get; }

            internal SmtpClientGetter(SmtpClient smtpClient)
            {
                Credentials = smtpClient.Credentials;
                DeliveryFormat = smtpClient.DeliveryFormat;
                DeliveryMethod = smtpClient.DeliveryMethod;
                EnableSsl = smtpClient.EnableSsl;
                Host = smtpClient.Host;
                Port = smtpClient.Port;
                TargetName = smtpClient.TargetName;
                Timeout = smtpClient.Timeout;
                UseDefaultCredentials = smtpClient.UseDefaultCredentials;

                smtpClient.Dispose();
            }

            internal SmtpClient Get()
            {
                SmtpClient result = new SmtpClient(Host, Port)
                {
                    Credentials = Credentials,
                    DeliveryFormat = DeliveryFormat,
                    DeliveryMethod = DeliveryMethod,
                    EnableSsl = EnableSsl,
                    PickupDirectoryLocation = PickupDirectoryLocation,
                    TargetName = TargetName,
                    Timeout = Timeout,
                    UseDefaultCredentials = UseDefaultCredentials
                };

                return result;
            }
        }
    }
}
