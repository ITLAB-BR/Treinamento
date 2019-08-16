using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Net.Mime;
using ITLab.Treinamento.Api.Core.Configuration;
using ITLab.Treinamento.Api.Core.Infrastructure.Mail;

namespace ITLab.Treinamento.Api.IdentityConfig
{
    public class EMailServiceIdentity : IIdentityMessageService
    {
        private EMailService eMailService;

        public EMailServiceIdentity()
        {
            eMailService = new EMailService();
        }
        public Task SendAsync(IdentityMessage message)
        {
            eMailService.SendAsync(message.Body, message.Subject, message.Destination);

            return Task.FromResult(0);
        }


    }
}