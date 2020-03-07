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
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        
        public ActionResult Contact()
        {
            ViewBag.Message = "Página de Contacto.";

            return View();
        }

        public ActionResult ErrorPage()
        {
            ViewBag.Message = "Pagina No Encontrada";

            return View();
        }
    }
}