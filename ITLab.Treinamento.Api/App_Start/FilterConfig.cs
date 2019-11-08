using System.Web;
using System.Web.Mvc;
using FluentSecurity;
using ITLab.Treinamento.Api.ActionFilters;

namespace ITLab.Treinamento.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleSecurityAttribute(), -1);
            filters.Add(new LogUncaughtExceptionMvcFilterAttribute());
        }
    }
}
