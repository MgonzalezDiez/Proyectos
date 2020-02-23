using Evaluacion360.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evaluacion360.Controllers
{
    public class HomeController : Controller
    {
        [AuthorizeUser(IdOperacion: 2)]
        public ActionResult Index()
        {
            return View();
        }
        [AuthorizeUser(IdOperacion: 3)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [AuthorizeUser(IdOperacion: 4)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ErrorPage()
        {
            ViewBag.Message = "Pagina No Encontrada";

            return View();
        }
    }
}