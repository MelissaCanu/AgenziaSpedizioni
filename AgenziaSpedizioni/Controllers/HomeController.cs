using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgenziaSpedizioni.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        // GET: Home/AdminHome
        //Authorize è un attributo che consente di specificare che l'accesso a un controller o a un'azione
        //è limitato solo agli utenti che soddisfano i requisiti specificati.
        [Authorize(Roles = "admin")]
        public ActionResult AdminHome()
        {
            return View();
        }

        //****************************************************

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}