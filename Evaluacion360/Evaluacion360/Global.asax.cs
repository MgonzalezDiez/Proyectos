using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Evaluacion360
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}
