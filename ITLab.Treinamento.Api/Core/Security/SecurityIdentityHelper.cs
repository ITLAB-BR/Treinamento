using ITLab.Treinamento.Api.Core.ExtensionsMethod;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Security
{
    public static class SecurityIdentityHelper
    {
        public const string AppIssuer = "ITLAB";
        public const string AppNameClaimType = "http://schemas.itlab.com.br";

        public static int CurrentUserId()
        {
            var userId = HttpContext.Current.User.Identity.GetUserId<int>();

            return userId;
        }
        public static string CurrentUsername()
        {
            var username = HttpContext.Current.User.Identity.GetUserName();

            return username;
        }
        public static bool CurrentUserHasAccessAllDataVisibility()
        {
            var claimsOfUser = (ClaimsIdentity)HttpContext.Current.User.Identity;
            var accessAllDataVisibility = SecurityIdentityHelper.GetAppClaimBoolValue(claimsOfUser.Claims, AppClaimTypes.ACCESS_ALL_DATA_VISIBILITY);

            return accessAllDataVisibility;
        }

        public static Claim CreateAppClaim(AppClaimTypes appClaim, bool value)
        {
            var newAppClaim = CreateAppClaim(appClaim.GetDescription().ToLower(), value.ToString(), "bool");
            return newAppClaim;
        }
        public static Claim CreateAppClaim(AppClaimTypes appClaim, int value)
        {
            var newAppClaim = CreateAppClaim(appClaim.GetDescription().ToLower(), value.ToString(), "int");
            return newAppClaim;
        }
        public static bool GetAppClaimBoolValue(IEnumerable<Claim> claims, AppClaimTypes appClaim)
        {
            var resultClaimValue = false;

            var claim = GetAppClaim(claims, appClaim);
            if (claim != null)
            {
                bool.TryParse(claim.Value, out resultClaimValue);
            }

            return resultClaimValue;
        }
        public static int GetAppClaimIntValue(IEnumerable<Claim> claims, AppClaimTypes appClaim)
        {
            var resultClaimValue = 0;

            var claim = GetAppClaim(claims, appClaim);
            if (claim != null)
            {
                int.TryParse(claim.Value, out resultClaimValue);
            }

            return resultClaimValue;
        }

        private static Claim CreateAppClaim(string type, string value, string valueType)
        {
            var newAppClaim = new Claim($"{AppNameClaimType}/{type}", value.ToString(), $"{AppNameClaimType}/{type}/#{valueType}");
            return newAppClaim;
        }
        private static Claim GetAppClaim(IEnumerable<Claim> claims, AppClaimTypes appClaim)
        {
            var claimType = $"{AppNameClaimType}/{appClaim.GetDescription().ToLower()}";
            var claim = claims.FirstOrDefault(c => c.Type == claimType);

            return claim;
        }
    }
}