using ITLab.Treinamento.Api.Core.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.Mail
{
    public class EMailService
    {
        public SmtpClient SmtpClient { get; set; }
        public MailMessage MailMessage { get; set; }

        public EMailService()
        {
            Configure(null);
        }

        public EMailService(string fromAddress, string fromDisplayName)
        {
            var mailAddress = new MailAddress(fromAddress, fromDisplayName);
            Configure(mailAddress);
        }

        public Task SendAsync(string body, string subject, string destination)
        {
            var text = body;
            var html = body;

            MailMessage.To.Add(new MailAddress(destination));
            MailMessage.Subject = subject;
            MailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            MailMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            SmtpClient.Send(MailMessage);

            return Task.FromResult(0);
        }


        private void Configure(MailAddress mailAddress)
        {
            SmtpClient = new SmtpClient();
            MailMessage = new MailMessage();

            SmtpClient.DeliveryMethod = SettingHelper.Get().SMTPDeliveryMethod;

            if (SmtpClient.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                SmtpClient.PickupDirectoryLocation = SettingHelper.Get().SMTPPickupDirectoryLocation;
                if (!Directory.Exists(SmtpClient.PickupDirectoryLocation))
                {
                    Directory.CreateDirectory(SmtpClient.PickupDirectoryLocation);
                }
            }
            else if (SmtpClient.DeliveryMethod == SmtpDeliveryMethod.Network)
            {
                var requiredConfigurations = new[] { SettingHelper.Get().SMTPServerHost, SettingHelper.Get().SMTPCredentialsUsername, SettingHelper.Get().SMTPCredentialsPassword };

                if (requiredConfigurations.Any(c => String.IsNullOrEmpty(c)))
                    throw new System.Exception(string.Format("SMTP Delivery Method not prepared {0}", SmtpClient.DeliveryMethod.ToString()));

                SmtpClient.Host = SettingHelper.Get().SMTPServerHost;
                SmtpClient.Port = SettingHelper.Get().SMTPServerPort;
                SmtpClient.EnableSsl = SettingHelper.Get().SMTPEnableSsl;
                SmtpClient.Credentials = new System.Net.NetworkCredential(SettingHelper.Get().SMTPCredentialsUsername, SettingHelper.Get().SMTPCredentialsPassword);
            }
            else
                throw new System.Exception(string.Format("SMTP Delivery Method not prepared {0}", SmtpClient.DeliveryMethod.ToString()));

            if (mailAddress == null)
            {
                var teste = SettingHelper.Get().SMTPDefaultFromAddress;
                MailMessage.From = new MailAddress(teste);
            }
            else
                MailMessage.From = mailAddress;
        }
    }
}