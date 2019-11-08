using Microsoft.VisualStudio.TestTools.UnitTesting;
using ITLab.Treinamento.Api.Providers;
using ITLab.Treinamento.Api.Core.Entities.Security;
using System;
using ITLab.Treinamento.Api.Core.Configuration;

namespace ITLab.Treinamento.Api.Tests.App_Start.Providers
{
    [TestClass]
    public class ApplicationOAuthProviderTest
    {
        [TestCategory("Authentication"), TestMethod]
        public void AuthenticationTypeEnabledForUser_DataBaseEnabled()
        {
            var generalSeetings = new GeneralSettingsApp { AuthenticateDataBase = true };

            Assert.AreEqual(true, ApplicationOAuthProvider.AuthenticationTypeEnabledForUser(AuthenticationType.DataBase, generalSeetings));
        }
        [TestCategory("Authentication"), TestMethod]
        public void AuthenticationTypeEnabledForUser_DataBaseDisabled()
        {
            var generalSeetings = new GeneralSettingsApp { AuthenticateDataBase = false };

            Assert.AreEqual(false, ApplicationOAuthProvider.AuthenticationTypeEnabledForUser(AuthenticationType.DataBase, generalSeetings));
        }
        [TestCategory("Authentication"), TestMethod]
        public void AuthenticationTypeEnabledForUser_ActiveDirectoryEnabled()
        {
            var generalSeetings = new GeneralSettingsApp { AuthenticateActiveDirectory = true };

            Assert.AreEqual(true, ApplicationOAuthProvider.AuthenticationTypeEnabledForUser(AuthenticationType.ActiveDirectory, generalSeetings));
        }
        [TestCategory("Authentication"), TestMethod]
        public void AuthenticationTypeEnabledForUser_ActiveDirectoryDisabled()
        {

            var generalSeetings = new GeneralSettingsApp { AuthenticateActiveDirectory = false };

            Assert.AreEqual(false, ApplicationOAuthProvider.AuthenticationTypeEnabledForUser(AuthenticationType.ActiveDirectory, generalSeetings));
        }

        [TestCategory("Authentication"), TestMethod]
        public void CheckUser_UserNotFound()
        {
            var generalSeetings = new GeneralSettingsApp();

            var authenticateResult = ApplicationOAuthProvider.CheckUser(null, "user.test", generalSeetings);

            Assert.AreEqual(false, authenticateResult.CheckUserIsOk);
            Assert.AreEqual("alerts:error.user_not_found", authenticateResult.MessageCode);
        }

        [TestCategory("Authentication"), TestMethod]
        public void CheckUser_UserDisabled()
        {
            User user = new User() { UserName = "user.test", Active = false };

            var generalSeetings = new GeneralSettingsApp();

            var authenticateResult = ApplicationOAuthProvider.CheckUser(user, user.UserName, generalSeetings);

            Assert.AreEqual(false, authenticateResult.CheckUserIsOk);
            Assert.AreEqual("alerts:error.user_disabled", authenticateResult.MessageCode);
        }

        [TestCategory("Authentication"), TestMethod]
        public void CheckUser_AuthenticateTypeDataBaseDisabled()
        {
            User user = new User() { UserName = "user.test", Active = true, AuthenticationType = AuthenticationType.DataBase };

            var generalSeetings = new GeneralSettingsApp() { AuthenticateDataBase = false };

            var authenticateResult = ApplicationOAuthProvider.CheckUser(user, user.UserName, generalSeetings);

            Assert.AreEqual(false, authenticateResult.CheckUserIsOk);
            Assert.AreEqual("alerts:error.authentication_type_disabled", authenticateResult.MessageCode);
        }

        [TestCategory("Authentication"), TestMethod]
        public void CheckUser_AuthenticateTypeActiveDirectoryDisabled()
        {
            User user = new User() { UserName = "user.test", Active = true, AuthenticationType = AuthenticationType.ActiveDirectory };

            var generalSeetings = new GeneralSettingsApp() { AuthenticateActiveDirectory = false };

            var authenticateResult = ApplicationOAuthProvider.CheckUser(user, user.UserName, generalSeetings);

            Assert.AreEqual(false, authenticateResult.CheckUserIsOk);
            Assert.AreEqual("alerts:error.authentication_type_disabled", authenticateResult.MessageCode);
        }
        [TestCategory("Authentication"), TestMethod]
        public void CheckUser_AuthenticateTypeDataBaseEnbabled()
        {
            User user = new User() { UserName = "user.test", Active = true, AuthenticationType = AuthenticationType.DataBase };

            var generalSeetings = new GeneralSettingsApp() { AuthenticateDataBase = true };

            var authenticateResult = ApplicationOAuthProvider.CheckUser(user, user.UserName, generalSeetings);

            Assert.AreEqual(true, authenticateResult.CheckUserIsOk);
        }

        [TestCategory("Authentication"), TestMethod]
        public void CheckUser_AuthenticateTypeActiveDirectoryEnabled()
        {
            User user = new User() { UserName = "user.test", Active = true, AuthenticationType = AuthenticationType.ActiveDirectory };

            var generalSeetings = new GeneralSettingsApp() { AuthenticateActiveDirectory = true };

            var authenticateResult = ApplicationOAuthProvider.CheckUser(user, user.UserName, generalSeetings);

            Assert.AreEqual(true, authenticateResult.CheckUserIsOk);
        }

        [TestCategory("Authentication"), TestMethod]
        public void CheckAuthenticatedInDataBase_UserAuthenticatedSuccess()
        {
            var signInStatus = Microsoft.AspNet.Identity.Owin.SignInStatus.Success;
            var AuthenticateResult = new AuthenticateResult();
            var user = new User() { LastPasswordChangedDate = DateTime.Now };

            var authenticateResult = ApplicationOAuthProvider.CheckAuthenticatedInDataBase(user, AuthenticateResult, signInStatus);

            Assert.AreEqual(true, authenticateResult.IsAuthenticated);
        }

        [TestCategory("Authentication"), TestMethod]
        public void CheckAuthenticatedInDataBase_UserPasswordExpired()
        {
            var signInStatus = Microsoft.AspNet.Identity.Owin.SignInStatus.Success;
            var AuthenticateResult = new AuthenticateResult();
            var settings = new GeneralSettingsApp {
                PasswordExpiresInDays = 1
            };
            SettingHelper.AddInCache(settings);

            var user = new User()
            {
                AuthenticationType = AuthenticationType.DataBase,
                LastPasswordChangedDate = DateTime.Now.Date.AddDays(-(SettingHelper.Get().PasswordExpiresInDays + 1))
            };

            var authenticateResult = ApplicationOAuthProvider.CheckAuthenticatedInDataBase(user, AuthenticateResult, signInStatus);

            Assert.AreEqual(false, authenticateResult.IsAuthenticated);
            Assert.AreEqual("alerts:warning.user_password_expired", authenticateResult.MessageCode);
        }

        [TestCategory("Authentication"), TestMethod]
        public void CheckAuthenticatedInDataBase_UserPasswordNotExpired()
        {
            var signInStatus = Microsoft.AspNet.Identity.Owin.SignInStatus.Success;
            var AuthenticateResult = new AuthenticateResult();
            var settings = new GeneralSettingsApp
            {
                PasswordExpiresInDays = 2
            };
            SettingHelper.AddInCache(settings);

            var user = new User()
            {
                AuthenticationType = AuthenticationType.DataBase,
                LastPasswordChangedDate = DateTime.Now.Date.AddDays(-1)
            };

            var authenticateResult = ApplicationOAuthProvider.CheckAuthenticatedInDataBase(user, AuthenticateResult, signInStatus);

            Assert.AreEqual(true, authenticateResult.IsAuthenticated);
        }

        [TestCategory("Authentication"), TestMethod]
        public void CheckAuthenticatedInDataBase_UserLocked()
        {
            var signInStatus = Microsoft.AspNet.Identity.Owin.SignInStatus.LockedOut;
            var AuthenticateResult = new AuthenticateResult();
            var user = new User();

            var authenticateResult = ApplicationOAuthProvider.CheckAuthenticatedInDataBase(user, AuthenticateResult, signInStatus);

            Assert.AreEqual(false, authenticateResult.IsAuthenticated);
            Assert.AreEqual("alerts:error.user_locked", authenticateResult.MessageCode);
        }
        [TestCategory("Authentication"), TestMethod]
        public void CheckAuthenticatedInDataBase_RequiresVerification()
        {
            var signInStatus = Microsoft.AspNet.Identity.Owin.SignInStatus.RequiresVerification;
            var AuthenticateResult = new AuthenticateResult();
            var user = new User();

            var authenticateResult = ApplicationOAuthProvider.CheckAuthenticatedInDataBase(user, AuthenticateResult, signInStatus);

            Assert.AreEqual(false, authenticateResult.IsAuthenticated);
            Assert.AreEqual("alerts:error.user_requiresVerification", authenticateResult.MessageCode);
        }
        [TestCategory("Authentication"), TestMethod]
        public void CheckAuthenticatedInDataBase_Failure()
        {
            var signInStatus = Microsoft.AspNet.Identity.Owin.SignInStatus.Failure;
            var AuthenticateResult = new AuthenticateResult();
            var user = new User();

            var authenticateResult = ApplicationOAuthProvider.CheckAuthenticatedInDataBase(user, AuthenticateResult, signInStatus);

            Assert.AreEqual(false, authenticateResult.IsAuthenticated);
            Assert.AreEqual("alerts:error.invalid_grant", authenticateResult.MessageCode);
        }
    }
}
