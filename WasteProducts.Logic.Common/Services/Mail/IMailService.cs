using System;
using System.Threading.Tasks;

namespace WasteProducts.Logic.Common.Services.Mail
{
    public interface IMailService : IDisposable
    {
        string OurEmail { get; set; }

        Task SendAsync(string to, string subject, string body);

        bool IsValidEmail(string email);
    }
}
