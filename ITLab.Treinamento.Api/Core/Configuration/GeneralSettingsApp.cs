using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Configuration
{
    public enum ActiveDirectoryType
    {
        LocalMachine = 1,
        Server = 2
    }

    public class GeneralSettingsApp
    {
        //Password Policy Settings
        public int PasswordRequiredMinimumLength { get; set; }
        public bool PasswordRequireDigit { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireNonLetterOrDigit { get; set; }
        public int PasswordHistoryLimit { get; set; }
        public int PasswordExpiresInDays { get; set; }

        //Security Policy
        public int AccessTokenExpireTimeSpanInMinutes { get; set; }
        public bool UserLockoutEnabledByDefault { get; set; }
        public int DefaultAccountLockoutTimeInMinutes { get; set; }
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        //Authentication Method Settings
        public bool AuthenticateActiveDirectory { get; set; }
        public bool AuthenticateDataBase { get; set; }

        //Active Directory Settings
        public ActiveDirectoryType ActiveDirectoryType { get; set; }
        public string ActiveDirectoryDomain { get; set; }
        public string ActiveDirectoryDN { get; set; }


        //Email Settings
        public System.Net.Mail.SmtpDeliveryMethod SMTPDeliveryMethod { get; set; }
        public string SMTPPickupDirectoryLocation { get; set; }
        public string SMTPServerHost { get; set; }
        public int SMTPServerPort { get; set; }
        public bool SMTPEnableSsl { get; set; }
        public string SMTPCredentialsUsername { get; set; }
        public string SMTPCredentialsPassword { get; set; }
        public string SMTPDefaultFromAddress { get; set; }

        //Skin CSS Settings
        public string LayoutSkin { get; set; }

        //Upload Directory Temp
        public string UploadDirectoryTemp { get; set; }
    }
}