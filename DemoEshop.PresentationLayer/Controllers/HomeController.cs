using System.Web.Mvc;

namespace DemoEshop.PresentationLayer.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            return View();
        }
    }
}