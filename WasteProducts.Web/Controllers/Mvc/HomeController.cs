using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NLog;

namespace WasteProducts.Web.Controllers.Mvc
{
    public class HomeController : BaseMvcController
    {
        public HomeController(ILogger logger) : base(logger)
        {
        }

        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}