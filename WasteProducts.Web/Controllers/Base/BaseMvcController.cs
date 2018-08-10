using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using NLog;

namespace WasteProducts.Web.Controllers.Base
{
    public abstract class BaseMvcController : Controller
    {
        [Inject]
        public ILogger Logger { protected get; set; }
    }
}