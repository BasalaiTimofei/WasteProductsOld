using System;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.MailService
{
    public interface IMailService : IDisposable
    {
        IMailFactory MailFactory { get; }

        string OurEmail { get; set; }

        Task SendAsync(string to, string subject, string body);

        bool IsValidEmail(string email);
    }
}
