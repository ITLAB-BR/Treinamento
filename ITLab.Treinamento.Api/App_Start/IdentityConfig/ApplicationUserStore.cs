using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ITLab.Treinamento.Api.IdentityConfig
{
    public class ApplicationUserStore : UserStore<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public ApplicationUserStore(AppDbContext context)
            : base(context) { }

        public override async Task CreateAsync(User user)
        {
            await base.CreateAsync(user);

            if (user.AuthenticationType == AuthenticationType.DataBase)
                await CustomManagementPasswordAsync(user, user.PasswordHash);
        }

        public Task CustomManagementPasswordAsync(User user, string password)
        {
            AddToPreviousPasswords(user, password);
            user.LastPasswordChangedDate = DateTime.Now;
            return UpdateAsync(user);
        }

        private static void AddToPreviousPasswords(User user, string password)
        {
            user.PreviousUserPasswords.Add(new PreviousUserPasswords { UserId = user.Id, PasswordHash = password, CreationUser = user.UserName });
        }
    }
}