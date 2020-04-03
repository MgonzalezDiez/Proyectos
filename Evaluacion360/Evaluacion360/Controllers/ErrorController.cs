using Evaluacion360.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evaluacion360.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [HttpGet]
        //[AuthorizeUser(IdOperacion: 1)]
        [AllowAnonymous]
        public ActionResult UnAuthorizedOperation(string Operation, string Module, string msgErrorException)
        {
            ViewBag.Operation = Operation;
            ViewBag.Module = Module;
            ViewBag.msgErrorException = msgErrorException;
            return View();
        }

        [HttpGet]
        [AuthorizeUser(IdOperacion: 1)]
        public ActionResult Error404()
        {
            return View();
        }
    }
}