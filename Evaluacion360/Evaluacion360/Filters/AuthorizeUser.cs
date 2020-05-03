using Evaluacion360.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evaluacion360.Filters
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class AuthorizeUser : AuthorizeAttribute
    {
        private Usuarios oUsuarios;
        private readonly BD_EvaluacionEntities db = new BD_EvaluacionEntities();
        private readonly int IdOperacion;

        public AuthorizeUser(int IdOperacion = 0)
        {
            this.IdOperacion = IdOperacion;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string NombreOperacion = string.Empty;
            string NombreModulo = string.Empty;
            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(
                                    typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization)
            {
                try
                {
                    oUsuarios = (Usuarios)HttpContext.Current.Session["User"];
                    var lstMyOperation = from m in db.Rol_Operacion
                                         join o in db.Operacion on m.IdOperacion equals o.Id
                                         where m.IdRol == oUsuarios.Tipo_Usuario && m.IdOperacion == IdOperacion
                                         select m;
                    if (lstMyOperation.Count() == 0)
                    {
                        var oOperation = db.Rol_Operacion.Find(IdOperacion);
                        NombreOperacion = GetNombreOperacion(IdOperacion);
                        filterContext.HttpContext.Response.Redirect("~/Error/UnAuthorizedOperation?Operacion = " + NombreOperacion);
                        //filterContext.Result = new RedirectResult("~/Error/UnAuthorizedOperation?Operacion = " + NombreOperacion);
                    }
                }
                catch (Exception ex)
                {
                    filterContext.HttpContext.Response.Redirect("~/Error/UnAuthorizedOperation?Error = " + ex.Message);
                    //filterContext.Result = new RedirectResult("~/Error/UnAuthorizedOperation?Error = " + ex.Message);
                }
            }
        }

        public string GetNombreOperacion(int idOperacion)
        {
            var op = from ope in db.Operacion
                     where ope.Id == idOperacion
                     select ope.Nombre;
            string NombreOperacion;
            try
            {
                NombreOperacion = op.First();
            }
            catch (Exception)
            {
                NombreOperacion = "";
            }
            return NombreOperacion;
        }
    }
}