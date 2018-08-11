using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using WasteProducts.Logic.Common.Services;

namespace WasteProducts.Logic.Services
{
    public class MailService : IMailService
    {
        private readonly SmtpClient _smtpClient;

        public MailService(SmtpClient smtpClient, IMailFactory mailFactory)
        {
            _smtpClient = smtpClient;
            MailFactory = mailFactory;
        }

        public IMailFactory MailFactory { get; }

        public void Dispose()
        {
            _smtpClient.Dispose();
        }

        public void Send(string from, string to, string subject, string body)
        {
            MailMessage message = null;

            try
            {
                message = MailFactory.Create(from, to, subject, body);
                _smtpClient.Send(message);
            }
            //TODO Add exceptions handling here
            finally
            {
                message.Dispose();
            }

        }
    }
}
