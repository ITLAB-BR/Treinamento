using System.Data.Entity.ModelConfiguration;
using ITLab.Treinamento.Api.Core.Entities.Security;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Security.Configurations
{
    public class UserClaimConfiguration : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimConfiguration()
        {
            ToTable("UserClaims", "AppSecurity");
            Property(p => p.Id).HasColumnName("UserClaimId");

            HasKey(c => c.Id);
        }
    }
}