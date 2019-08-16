using System;
using System.Linq;
using System.Threading.Tasks;
using ITLab.Treinamento.Api.Core.Configuration;
using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Data.Entity;

namespace ITLab.Treinamento.Api.IdentityConfig
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<User, int>
    {
        public ApplicationUserManager(IUserStore<User, int> store)
            : base(store)
        {
        }

        public static bool AuthenticateTypeIsSet()
        {
            var authenticateTypeIsSet = (SettingHelper.Get().AuthenticateDataBase || SettingHelper.Get().AuthenticateActiveDirectory);
            return authenticateTypeIsSet;
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            //EnableCORS(context);

            var manager = new ApplicationUserManager(new ApplicationUserStore(context.Get<AppDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<User, int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 2;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<User, int>
            {
                MessageFormat = "Your security code is {0}"
            });

            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<User, int>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });

            manager.EmailService = new EMailServiceIdentity();
            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"));
            }

            return manager;
        }

        public override async Task<IdentityResult> ChangePasswordAsync(int userId, string currentPassword,
            string newPassword)
        {
            PasswordValidator = RefreshPasswordValidator();

            if (IsPreviousPassword(userId, newPassword))
            {
                return await Task.FromResult(IdentityResult.Failed("Cannot reuse old password"));
            }

            var result = await base.ChangePasswordAsync(userId, currentPassword, newPassword);
            if (!result.Succeeded) return result;

            var store = Store as ApplicationUserStore;
            var user = await FindByIdAsync(userId);
            var newPasswordHash = PasswordHasher.HashPassword(newPassword);
            if (store != null) await store.CustomManagementPasswordAsync(user, newPasswordHash);

            return result;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(int userId, string token, string newPassword)
        {
            PasswordValidator = RefreshPasswordValidator();

            if (IsPreviousPassword(userId, newPassword))
            {
                return await Task.FromResult(IdentityResult.Failed("Cannot reuse old password"));
            }

            var result = await base.ResetPasswordAsync(userId, token, newPassword);
            if (!result.Succeeded) return result;

            var store = Store as ApplicationUserStore;
            if (store != null)
                await store.CustomManagementPasswordAsync(await FindByIdAsync(userId), PasswordHasher.HashPassword(newPassword));

            return result;
        }

        private bool IsPreviousPassword(int userId, string newPassword)
        {
            var passwordHistoryLimit = SettingHelper.Get().PasswordHistoryLimit;

            var IsResusePassword = Users.Where(u => u.Id == userId)
                       .SelectMany(u => u.PreviousUserPasswords)
                       .OrderByDescending(x => x.CreationDate)
                       .Take(passwordHistoryLimit)
                       .Select(x => x.PasswordHash)
                       .ToArray()
                       .Any(x => PasswordHasher.VerifyHashedPassword(x, newPassword) != PasswordVerificationResult.Failed);

            return IsResusePassword;
        }

        public override async Task<IdentityResult> CreateAsync(User user, string password)
        {
            PasswordValidator = RefreshPasswordValidator();

            var result = await base.CreateAsync(user, password);

            return result;
        }

        private static PasswordValidator RefreshPasswordValidator()
        {
            var settings = SettingHelper.Get();

            var passwordValidator = new PasswordValidator
            {
                RequiredLength = settings.PasswordRequiredMinimumLength,
                RequireNonLetterOrDigit = settings.PasswordRequireNonLetterOrDigit,
                RequireDigit = settings.PasswordRequireDigit,
                RequireLowercase = settings.PasswordRequireLowercase,
                RequireUppercase = settings.PasswordRequireUppercase
            };

            return passwordValidator;
        }

        public async Task<User> FindUserAsync(string userName)
        {
            var user = await base.FindByNameAsync(userName);
            if (user == null) user = await base.FindByEmailAsync(userName);

            return user;
        }
    }
}