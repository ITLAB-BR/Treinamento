using System.Security.Claims;
using System.Threading.Tasks;
using ITLab.Treinamento.Api.Core.Entities.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using ITLab.Treinamento.Api.Core.Configuration;
using System;

namespace ITLab.Treinamento.Api.IdentityConfig
{
    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, int>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            RefreshSignInOptions(userManager);
        }

        private static void RefreshSignInOptions(ApplicationUserManager userManager)
        {
            userManager.UserLockoutEnabledByDefault = SettingHelper.Get().UserLockoutEnabledByDefault;
            userManager.DefaultAccountLockoutTimeSpan = new TimeSpan(0, SettingHelper.Get().DefaultAccountLockoutTimeInMinutes, 0);
            userManager.MaxFailedAccessAttemptsBeforeLockout = SettingHelper.Get().MaxFailedAccessAttemptsBeforeLockout;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(User user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager, OAuthDefaults.AuthenticationType);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}