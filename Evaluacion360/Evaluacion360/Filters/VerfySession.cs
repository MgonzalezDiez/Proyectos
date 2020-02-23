using Evaluacion360.Controllers;
using Evaluacion360.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace Evaluacion360.Filters
{
    public class VerfySession: ActionFilterAttribute
    {
        private Usuarios oUser;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                oUser = (Usuarios)HttpContext.Current.Session["User"];
                if (oUser == null)
                {
                    if (filterContext.Controller is AccessController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Access/LogIn");
                    }
                }
                else
                {
                    if (filterContext.Controller is AccessController == true)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Home/Index");
                    }
                }
                base.OnActionExecuting(filterContext);
            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Access/LogIn");
            }
            
        }

    }
}