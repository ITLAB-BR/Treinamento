using System.Data.Entity.ModelConfiguration;
using ITLab.Treinamento.Api.Core.Entities.Security;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Security.Configurations
{
    public class UserLoginConfiguration : EntityTypeConfiguration<UserLogin>
    {
        public UserLoginConfiguration()
        {
            ToTable(nameof(UserLogin), "AppSecurity");

            HasKey(p => new { p.LoginProvider, p.ProviderKey, p.UserId });
        }
    }
}