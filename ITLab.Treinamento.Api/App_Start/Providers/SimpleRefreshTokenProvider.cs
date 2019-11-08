using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace ITLab.Treinamento.Api.Providers
{
    public class SimpleRefreshTokenProvider : IAuthenticationTokenProvider
    {
        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientId = context.Ticket.Properties.Dictionary["as:client_id"];
            var username = context.Ticket.Properties.Dictionary["userName"];

            if (string.IsNullOrEmpty(clientId))
            {
                return;
            }

            var refreshTokenId = Guid.NewGuid().ToString("n");

            using (var dbContext = new AppDbContext().WithUsername(username))
            {
                var refreshTokenLifeTime = context.OwinContext.Get<string>("as:clientRefreshTokenLifeTime");

                var refreshToken = new APIClientRefreshToken
                {
                    Id = Core.Security.SecurityHelper.GetHash(refreshTokenId),
                    Subject = context.Ticket.Identity.Name,
                    IssuedUTC = DateTime.UtcNow,
                    ExpiresUTC = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifeTime))
                };

                context.Ticket.Properties.IssuedUtc = refreshToken.IssuedUTC;
                context.Ticket.Properties.ExpiresUtc = refreshToken.ExpiresUTC;

                refreshToken.ProtectedTicket = context.SerializeTicket();

                var apiClient = new APIClients { Id = clientId };
                dbContext.APIClients.Attach(apiClient);
                apiClient.AddRefreshToken(refreshToken);

                await dbContext.SaveChangesAsync();

                context.SetToken(refreshTokenId);
            }
        }
        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            var hashedTokenId = Core.Security.SecurityHelper.GetHash(context.Token);

            using (var dbContext = new AppDbContext())
            {
                var refreshToken = await dbContext.APIClientRefreshToken.FirstOrDefaultAsync(r=>r.Id == hashedTokenId);
                if (refreshToken !=null)
                {
                    context.DeserializeTicket(refreshToken.ProtectedTicket);
                    dbContext.APIClientRefreshToken.Remove(refreshToken);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
        public void Create(AuthenticationTokenCreateContext context)
        {
            throw new NotImplementedException();
        }
        public void Receive(AuthenticationTokenReceiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}