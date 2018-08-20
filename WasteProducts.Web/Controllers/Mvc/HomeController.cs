using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NLog;
using WasteProducts.Web.App_Start;
using LogLevel = NLog.LogLevel;

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