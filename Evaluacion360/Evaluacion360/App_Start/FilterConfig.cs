using System.Web.Mvc;

namespace Evaluacion360
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new Filters.VerfySession());
        }
    }
}
