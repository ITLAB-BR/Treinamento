using ITLab.Treinamento.Api.Core.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITLab.Treinamento.Api.Tests.Core.Configuration
{
    [TestClass]
    public class SettingHelperTest
    {
        [TestMethod]
        public void ParseObjectToSettingDataBaseTest()
        {
            var settings = new GeneralSettingsApp
            {
                PasswordRequiredMinimumLength = 3,
                PasswordRequireDigit = false,
                PasswordRequireLowercase = false,
                PasswordRequireUppercase = false,
                PasswordRequireNonLetterOrDigit = false,
                PasswordHistoryLimit = 3,

                AccessTokenExpireTimeSpanInMinutes = 30,
                UserLockoutEnabledByDefault = true,
                DefaultAccountLockoutTimeInMinutes = 2,
                MaxFailedAccessAttemptsBeforeLockout = 4,

                AuthenticateDataBase = true,
                AuthenticateActiveDirectory = true,

                ActiveDirectoryType = ActiveDirectoryType.Server,
                ActiveDirectoryDomain = "itlab.local",
                ActiveDirectoryDN = "dc=itlab,dc=local",

                SMTPDeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory,
                SMTPPickupDirectoryLocation = @"c:\temp\email"
            };

            var generalSettingsDataBase = SettingHelper.ParseObjectToSettingDataBase(settings);

            foreach (var item in settings.GetType().GetProperties())
            {
                var settingItem = generalSettingsDataBase.Where(x => x.SettingName == item.Name).SingleOrDefault();
                Assert.IsNotNull(settingItem);
            }
        }
    }
}
