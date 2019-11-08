using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.IdentityConfig;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Entity;

namespace ITLab.Treinamento.Api.IdentityConfig
{
    // Configurando o RoleManager utilizado na aplicação.
    public class ApplicationRoleManager : RoleManager<Role, int> 
    {
        public ApplicationRoleManager(IRoleStore<Role, int> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<Role, int, UserRole>(context.Get<AppDbContext>()));
        }
    }
}