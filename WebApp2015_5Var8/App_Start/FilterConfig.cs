using System.Web;
using System.Web.Mvc;

namespace WebApp2015_5Var8
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
