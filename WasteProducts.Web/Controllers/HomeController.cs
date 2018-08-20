using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;

namespace WasteProducts.Web.Controllers
{
    public class HomeController : BaseMvcController
    {
        public HomeController(ILogger logger) : base(logger)
        {

        }

        public async Task<ActionResult> Index()
        {
            Logger.Trace("Trace");
            Logger.Debug("Debug");
            Logger.Info("Trace");
            Logger.Warn("Trace");
            Logger.Error(new Exception("Exc"), "Error message");
            Logger.Fatal(new Exception("Fatal"), "Fatal message");

            return View();
        }



    }
}