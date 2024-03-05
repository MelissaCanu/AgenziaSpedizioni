using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AgenziaSpedizioni.Controllers
{
    public class SpedizioniController : Controller
    {
        // GET: Spedizioni
        public ActionResult Index()
        {
            return View();
        }
    }
}