using ITLab.Treinamento.Api.Core.Configuration;
using ITLab.Treinamento.Api.Core.Entities.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITLab.Treinamento.Api.Tests.Core.Entities.Security
{
    [TestClass]
    public class UserTest
    {
        [TestMethod]
        public void User_UserPasswordExpired_PasswordHasChange_ScenarioOne()
        {
            //Arrange
            var settingPasswordExpiresInDays = 2;
            var userCreationDate = DateTime.Now.AddDays(-3);
            var userLastPasswordChangeDate = DateTime.Now.AddDays(-2);

            //Expected
            var resultExpected1 = true;
            var resultExpected2 = 0;
            var resultExpected3 = DateTime.Now.Date;

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.DataBase, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }

        [TestMethod]
        public void User_UserPasswordExpired_PasswordHasChange_ScenarioTwo()
        {
            //Arrange
            var settingPasswordExpiresInDays = 2;
            var userCreationDate = DateTime.Now.AddDays(-11);
            var userLastPasswordChangeDate = DateTime.Now.AddDays(-10);

            //Expected
            var resultExpected1 = true;
            var resultExpected2 = 0;
            var resultExpected3 = DateTime.Now.Date;

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.DataBase, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }

        [TestMethod]
        public void User_UserPasswordNotExpired_PasswordHasChange()
        {
            //Arrange
            var settingPasswordExpiresInDays = 2;
            var userCreationDate = DateTime.Now.AddDays(-2);
            var userLastPasswordChangeDate = DateTime.Now.AddDays(-1);

            //Expected
            var resultExpected1 = false;
            var resultExpected2 = 1;
            var resultExpected3 = DateTime.Now.Date.AddDays(+1);

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.DataBase, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }

        [TestMethod]
        public void User_UserPasswordExpired_PasswordNotHasChange_ScenarioOne()
        {
            //Arrange
            var settingPasswordExpiresInDays = 2;
            var userCreationDate = DateTime.Now.AddDays(-2);
            DateTime? userLastPasswordChangeDate = null;

            //Expected
            var resultExpected1 = true;
            var resultExpected2 = 0;
            var resultExpected3 = DateTime.Now.Date;

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.DataBase, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }

        [TestMethod]
        public void User_UserPasswordExpired_PasswordNotHasChange_ScenarioTwo()
        {
            //Arrange
            var settingPasswordExpiresInDays = 2;
            var userCreationDate = DateTime.Now.AddDays(-10);
            DateTime? userLastPasswordChangeDate = null;

            //Expected
            var resultExpected1 = true;
            var resultExpected2 = 0;
            var resultExpected3 = DateTime.Now.Date;

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.DataBase, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }

        [TestMethod]
        public void User_UserPasswordNotExpired_PasswordNotHasChange()
        {
            //Arrange
            var settingPasswordExpiresInDays = 2;
            var userCreationDate = DateTime.Now.AddDays(-1);
            DateTime? userLastPasswordChangeDate = null;

            //Expected
            var resultExpected1 = false;
            var resultExpected2 = 1;
            var resultExpected3 = DateTime.Now.Date.AddDays(+1);

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.DataBase, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }

        [TestMethod]
        public void User_PasswordExpireNotSet()
        {
            //Arrange
            var settingPasswordExpiresInDays = 0;
            var userCreationDate = DateTime.Now.AddDays(-3);
            var userLastPasswordChangeDate = DateTime.Now.AddDays(-2);

            //Expected
            var resultExpected1 = false;
            int? resultExpected2 = null;
            DateTime? resultExpected3 = null;

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.DataBase, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }

        [TestMethod]
        public void User_TypeActiveDirectory_ScenarioOne()
        {
            //Arrange
            var settingPasswordExpiresInDays = 0;
            var userCreationDate = DateTime.Now.AddDays(-3);
            var userLastPasswordChangeDate = DateTime.Now.AddDays(-2);

            //Expected
            var resultExpected1 = false;
            int? resultExpected2 = null;
            DateTime? resultExpected3 = null;

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.ActiveDirectory, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }

        [TestMethod]
        public void User_TypeActiveDirectory_ScenarioTwo()
        {
            //Arrange
            var settingPasswordExpiresInDays = 2;
            var userCreationDate = DateTime.Now.AddDays(-3);
            var userLastPasswordChangeDate = DateTime.Now.AddDays(-2);

            //Expected
            var resultExpected1 = false;
            int? resultExpected2 = null;
            DateTime? resultExpected3 = null;

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.ActiveDirectory, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }

        [TestMethod]
        public void User_TypeActiveDirectory_ScenarioThree()
        {
            //Arrange
            var settingPasswordExpiresInDays = 2;
            var userCreationDate = DateTime.Now.AddDays(-3);
            var userLastPasswordChangeDate = DateTime.Now.AddDays(-1);

            //Expected
            var resultExpected1 = false;
            int? resultExpected2 = null;
            DateTime? resultExpected3 = null;

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.ActiveDirectory, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }
        [TestMethod]
        public void User_TypeActiveDirectory_ScenarioFour()
        {
            //Arrange
            var settingPasswordExpiresInDays = 3;
            var userCreationDate = DateTime.Now.AddDays(-3);
            DateTime? userLastPasswordChangeDate = null;

            //Expected
            var resultExpected1 = false;
            int? resultExpected2 = null;
            DateTime? resultExpected3 = null;

            //Act
            var user = ConfigUserTestPasswordExpire(settingPasswordExpiresInDays, AuthenticationType.ActiveDirectory, userCreationDate, userLastPasswordChangeDate);
            var result1 = user.IsPasswordExpired();
            var result2 = user.DaysLeftToChangePassword();
            var result3 = user.DateThatMustChangePassword();

            Assert.AreEqual(resultExpected1, result1);
            Assert.AreEqual(resultExpected2, result2);
            Assert.AreEqual(resultExpected3, result3);
        }
        private User ConfigUserTestPasswordExpire(int settingPasswordExpiresInDays, AuthenticationType authenticationType, DateTime creationDate, DateTime? lastPasswordChangeDate)
        {
            var settings = new GeneralSettingsApp
            {
                PasswordExpiresInDays = settingPasswordExpiresInDays
            };
            SettingHelper.AddInCache(settings);

            var user = new User
            {
                AuthenticationType = authenticationType,
                CreationDate = creationDate,
                LastPasswordChangedDate = lastPasswordChangeDate
            };

            return user;
        }
    }
}
