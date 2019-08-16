using ITLab.Treinamento.Api.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations.Security
{
    public class APIClientRefreshTokenConfiguration : EntityTypeConfiguration<APIClientRefreshToken>
    {
        public APIClientRefreshTokenConfiguration()
        {
            ToTable(nameof(APIClientRefreshToken), "AppSecurity");

            HasKey(k => k.Id);
            Property(p => p.Id).HasColumnName("APIClientRefreshTokenId")
                .IsRequired()
                .HasMaxLength(44);

            Property(p => p.Subject)
                .IsRequired()
                .HasMaxLength(50);

            Property(p => p.ProtectedTicket)
                .IsRequired();
        }
    }
}