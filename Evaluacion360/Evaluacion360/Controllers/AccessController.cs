using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Evaluacion360.Filters;
using Evaluacion360.Models;

namespace Evaluacion360.Controllers
{
    public class AccessController : Controller
    {
        private string mensaje = string.Empty;

        // GET: Access
        public ActionResult LogIn(string Mensaje, bool State=false)
        {
            ViewBag.Status = true;
            if (Mensaje != null)
            {
                ViewBag.Status = State;
                ViewBag.Message = Mensaje;
            }
            //mensaje = string.Empty;
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(Usuarios user)
        {
            try
            {
                using (BD_EvaluacionEntities Db = new BD_EvaluacionEntities())
                {
                    user.PASS = Crypto.Hash(user.PASS);
                    user.Nombre_Usuario = user.Nombre_Usuario.ToUpper();
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
                    mensaje = "Usuario o Password Inválida";
                    bool state = false;
                    return RedirectToAction("LogIn", "Access", new { mensaje, state });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

    }
}