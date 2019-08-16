using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity;
using ITLab.Treinamento.Api.Core.Security.Authorization.AppControllers;
using log4net.Config;

namespace ITLab.Treinamento.Api
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            SecurityConfigurator.Configure(configuration =>
            {
                configuration.GetAuthenticationStatusFrom(() => HttpContext.Current.User.Identity.IsAuthenticated);

                //Chamada para as configurações de segurança da aplicação
                ControllersConfiguration.Configure(configuration);

                //Chamada para as configurações de segurança das controllers especificas de autenticação/autorização
                //Ex.: Criação e configuração de usuários, criação e configurações de roles
                Core.Security.Authorization.SecurityControllers.ControllersConfiguration.Configure(configuration);
            });

            XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}