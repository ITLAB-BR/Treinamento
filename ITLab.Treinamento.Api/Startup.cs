using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ITLab.Treinamento.Api.Startup))]
namespace ITLab.Treinamento.Api
{
    public partial class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
        }
    }
}
