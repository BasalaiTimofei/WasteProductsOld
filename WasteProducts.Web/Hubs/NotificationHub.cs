using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using NLog;

namespace WasteProducts.Web.Hubs
{

    public class NotificationHub : Hub
    {
        private readonly ILogger _logger;

        public NotificationHub(ILogger logger)
        {
            _logger = logger;
        }
        
    }
}