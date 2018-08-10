using System.Web;
using System.Web.Mvc;


namespace WasteProducts.Web.Controllers
{
    public class HomeController : Base.BaseMvcController
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Donate()
        {
            Logger.Info("O_O We have a donate page");

            throw new HttpException(403, "Доступ запрещён!");

            return View();
        }
    }
}