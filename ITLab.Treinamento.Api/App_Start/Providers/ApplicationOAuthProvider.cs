using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.IdentityConfig;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System.DirectoryServices.AccountManagement;
using ITLab.Treinamento.Api.Core.Configuration;
using ITLab.Treinamento.Api.Core.Security;
using System.Linq;

namespace ITLab.Treinamento.Api.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        public AuthenticateResult authenticateResult;

        /// <summary>
        ///     Realiza a autenticação do usuário
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            authenticateResult = new AuthenticateResult { IsAuthenticated = false, Username = context.UserName };

            if (!ApplicationUserManager.AuthenticateTypeIsSet())
            {
                authenticateResult.IsAuthenticated = false;
                authenticateResult.MessageCode = "alerts:error.authentication_type_not_set";
                authenticateResult.MessageDescription = "Authentication not set(see General Settings)";
            }
            else
            {
                var user = await userManager.FindUserAsync(context.UserName);

                authenticateResult = CheckUser(user, context.UserName, SettingHelper.Get());
                if (authenticateResult.CheckUserIsOk && user.AuthenticationType == AuthenticationType.DataBase)
                {
                    var signInManager = context.OwinContext.Get<ApplicationSignInManager>();

                    var result = await AuthenticateDataBaseAsync(signInManager, user.UserName, context.Password);

                    authenticateResult = CheckAuthenticatedInDataBase(user, authenticateResult, result);

                    if (authenticateResult.IsAuthenticated)
                        await userManager.ResetAccessFailedCountAsync(user.Id);  // Zerando contador de logins errados.
                }
                else if (authenticateResult.CheckUserIsOk && user.AuthenticationType == AuthenticationType.ActiveDirectory)
                {
                    authenticateResult.IsAuthenticated = AuthenticateActiveDirectory(context.UserName, context.Password);
                    if (!authenticateResult.IsAuthenticated)
                    {
                        authenticateResult.MessageCode = "alerts:error.invalid_grant";
                        authenticateResult.MessageDescription = "The user name or password is incorrect.";
                    }
                }

                if (authenticateResult.IsAuthenticated)
                {
                    authenticateResult.Username = user.UserName;

                    var oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, OAuthDefaults.AuthenticationType);
                    var cookiesIdentity = await user.GenerateUserIdentityAsync(userManager, CookieAuthenticationDefaults.AuthenticationType);

                    AddCustomClaims(oAuthIdentity, user);

                    var properties = CreateProperties(user.UserName, context.ClientId);
                    var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                    context.Request.Context.Authentication.SignIn(cookiesIdentity);
                }
            }


            SaveAccessLog(authenticateResult);

            if (!authenticateResult.IsAuthenticated)
            {
                context.SetError(authenticateResult.MessageCode, authenticateResult.MessageDescription);
            }
        }

        public override Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClient = context.Ticket.Properties.Dictionary["as:client_id"];
            var currentClient = context.ClientId;

            if (originalClient != currentClient)
            {
                context.SetError("alerts:error.invalid_clientId", "Refresh token is issued to a different clientId.");
                return Task.FromResult<object>(null);
            }

            var newIdentity = new ClaimsIdentity(context.Ticket.Identity);

            var newClaim = newIdentity.Claims.FirstOrDefault(c => c.Type == "newClaim");
            if (newClaim != null)
            {
                newIdentity.RemoveClaim(newClaim);
            }
            newIdentity.AddClaim(new Claim(nameof(newClaim), "newValue"));

            var newTicket = new AuthenticationTicket(newIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

            return Task.FromResult<object>(null);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (var property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            var apiClientId = string.Empty;
            var apiClientSecret = string.Empty;
            APIClients apiClient = null;

            if (!context.TryGetBasicCredentials(out apiClientId, out apiClientSecret))
            {
                context.TryGetFormCredentials(out apiClientId, out apiClientSecret);
            }

            if (context.ClientId == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context
                //if you want to force sending clientId/secrects once obtain access tokens.
                context.Validated();
                context.SetError("alerts:error.invalid_clientId", "ClientId should be sent.");
                return Task.FromResult<object>(null);
            }

            using (var dbContext = new AppDbContext())
            {
                apiClient = dbContext.APIClients.Find(apiClientId);
            }

            if (apiClient == null)
            {
                context.SetError("alerts:error.invalid_clientId", $"Client '{context.ClientId}' is not registered in the system.");
                return Task.FromResult<object>(null);
            }

            if (apiClient.Type == APIClientTypes.NativeConfidencial)
            {
                if (string.IsNullOrWhiteSpace(apiClientSecret))
                {
                    context.SetError("alerts:error.invalid_clientId", "Client secret should be sent.");
                    return Task.FromResult<object>(null);
                }
                else
                {
                    if (apiClient.Secret != SecurityHelper.GetHash(apiClientSecret))
                    {
                        context.SetError("alerts:error.invalid_clientId", "Client secret is invalid.");
                        return Task.FromResult<object>(null);
                    }
                }
            }

            if (!apiClient.Active)
            {
                context.SetError("alerts:error.invalid_clientId", "Client is inactive.");
                return Task.FromResult<object>(null);
            }

            if (apiClient.AllowedOrigin == "*")
            {
                context.OwinContext.Set<string>("as:clientAllowedOrigin", "*");
            }
            else
            {
                context.OwinContext.Set<string>("as:clientAllowedOrigin", new Uri(apiClient.AllowedOrigin).GetLeftPart(UriPartial.Authority));
            }

            context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", apiClient.RefreshTokenLifeTimeInMinutes.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            if (context.ClientId != null) return Task.FromResult<object>(null);

            var expectedRootUri = new Uri(context.Request.Uri, "/");

            if (expectedRootUri.AbsoluteUri == context.RedirectUri)
            {
                context.Validated();
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, string client_id)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {nameof(userName), userName},
                { "as:client_id", client_id }
            };
            return new AuthenticationProperties(data);
        }

        public static AuthenticateResult CheckAuthenticatedInDataBase(User user, AuthenticateResult authenticateResult, SignInStatus result)
        {
            switch (result)
            {
                case SignInStatus.Success:
                    if (user.IsPasswordExpired())
                    {
                        authenticateResult.IsAuthenticated = false;
                        authenticateResult.MessageCode = "alerts:warning.user_password_expired";
                        authenticateResult.MessageDescription = "The password is expired";
                    }
                    else
                    {
                        authenticateResult.IsAuthenticated = true;
                    }
                    break;
                case SignInStatus.LockedOut:
                    authenticateResult.IsAuthenticated = false;
                    authenticateResult.MessageCode = "alerts:error.user_locked";
                    authenticateResult.MessageDescription = "The user is locked.";
                    break;
                //Caso o two factory esteja habilitada virá um RequiresVerification e não um Success.
                case SignInStatus.RequiresVerification:
                    authenticateResult.IsAuthenticated = false;
                    authenticateResult.MessageCode = "alerts:error.user_requiresVerification";
                    authenticateResult.MessageDescription = "The user must enter the second authentication factor.";
                    break;
                case SignInStatus.Failure:
                default:
                    authenticateResult.IsAuthenticated = false;
                    authenticateResult.MessageCode = "alerts:error.invalid_grant";
                    authenticateResult.MessageDescription = "The user name or password is incorrect.";
                    break;
            }

            return authenticateResult;
        }

        public static AuthenticateResult CheckUser(User user, string username, GeneralSettingsApp generalSetting)
        {
            var authenticateResult = new AuthenticateResult
            {
                Username = username,
                IsAuthenticated = false,
                CheckUserIsOk = false
            };

            if (user == null)
            {
                authenticateResult.MessageCode = "alerts:error.user_not_found";
                authenticateResult.MessageDescription = "User not found.";
            }
            else if (!user.Active)
            {
                authenticateResult.MessageCode = "alerts:error.user_disabled";
                authenticateResult.MessageDescription = "The user is disabled.";
            }
            else if (!AuthenticationTypeEnabledForUser(user.AuthenticationType, generalSetting))
            {
                authenticateResult.MessageCode = "alerts:error.authentication_type_disabled";
                authenticateResult.MessageDescription = "Authentication type disabled";
            }
            else
                authenticateResult.CheckUserIsOk = true;

            return authenticateResult;
        }

        public static bool AuthenticationTypeEnabledForUser(AuthenticationType userAuthenticationType, GeneralSettingsApp generalSettings)
        {
            var result = (userAuthenticationType == AuthenticationType.DataBase && generalSettings.AuthenticateDataBase) || (userAuthenticationType == AuthenticationType.ActiveDirectory && generalSettings.AuthenticateActiveDirectory);
            return result;
        }

        private static void SaveAccessLog(AuthenticateResult authenticateResult)
        {
            var accessLog = new AccessLog
            {
                Login = authenticateResult.Username,
                Active = authenticateResult.IsAuthenticated,
                AttempAccessDateTime = DateTime.Now,
                ClientIP = HttpContext.Current.Request.UserHostAddress,
                MessageCode = authenticateResult.MessageCode,
                MessageDescription = authenticateResult.MessageDescription
            };

            using (var context = new AppDbContext())
            {
                context.AccessLog.Add(accessLog);
                context.SaveChanges();
            }
        }

        private async static Task<SignInStatus> AuthenticateDataBaseAsync(ApplicationSignInManager signInManager, string userName, string password)
        {
            var result = await signInManager.PasswordSignInAsync(userName, password, shouldLockout: true, isPersistent: false);

            return result;
        }

        private static bool AuthenticateActiveDirectory(string username, string password)
        {
            try
            {
                if (SettingHelper.Get().ActiveDirectoryType == ActiveDirectoryType.LocalMachine)
                {
                    using (var principalContext = new PrincipalContext(ContextType.Machine))
                    {
                        return principalContext.ValidateCredentials(username, password);
                    }
                }
                else if (SettingHelper.Get().ActiveDirectoryType == ActiveDirectoryType.Server)
                {
                    using (var principalContext = new PrincipalContext(ContextType.Domain,
                                                                                SettingHelper.Get().ActiveDirectoryDomain,
                                                                                SettingHelper.Get().ActiveDirectoryDN))
                    {
                        return principalContext.ValidateCredentials(username, password);
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("ActiveDirectoryType is not valid");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void AddCustomClaims(ClaimsIdentity claimIdentity, User user)
        {
            var claimAccessAllDataVisibility = SecurityIdentityHelper.CreateAppClaim(AppClaimTypes.ACCESS_ALL_DATA_VISIBILITY, user.AccessAllDataVisibility);

            claimIdentity.AddClaim(claimAccessAllDataVisibility);
        }
    }

    public class AuthenticateResult
    {
        public string Username { get; set; }
        public bool CheckUserIsOk { get; set; }
        public bool IsAuthenticated { get; set; }
        public string MessageCode { get; set; }
        public string MessageDescription { get; set; }
    }
}