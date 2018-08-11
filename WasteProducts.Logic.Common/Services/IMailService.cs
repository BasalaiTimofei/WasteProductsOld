using System;
using System.Net;
using System.Net.Mail;

namespace WasteProducts.Logic.Common.Services
{
    public interface IMailService : IDisposable
    {
        IMailFactory MailFactory { get; }

        void Send(string from, string to, string subject, string body);
    }
}
