using Evaluacion360.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Evaluacion360.Controllers
{
    public class AccessController : Controller
    {
        private string mensaje = string.Empty;

        // GET: Access
        [AllowAnonymous]
        public ActionResult LogIn(string Mensaje, bool State = false)
        {
            ViewBag.Status = true;
            if (Mensaje != null)
            {
                ViewBag.Status = State;
                ViewBag.Message = Mensaje;
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogIn(Usuarios user)
        {
            try
            {
                user.PASS = Crypto.Hash(user.PASS);
                user.Nombre_Usuario = user.Nombre_Usuario.ToUpper();
                using BD_EvaluacionEntities Db = new BD_EvaluacionEntities();
                var oUser = (from usr in Db.Usuarios
                             where usr.Nombre_Usuario.Trim().Equals(user.Nombre_Usuario) && usr.PASS.Trim().Equals(user.PASS)
                             select usr).FirstOrDefault();
                if (oUser != null)
                {
                    ViewBag.Mensaje = "";
                    Session["User"] = oUser;
                    Session["TipoUsuario"] = oUser.Tipo_Usuario;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    mensaje = "Usuario o Password Inválida";
                    bool state = false;
                    return RedirectToAction("LogIn", "Access", new { mensaje, state });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                Session["User"] = "";
                return View();

            }
        }

    }
}