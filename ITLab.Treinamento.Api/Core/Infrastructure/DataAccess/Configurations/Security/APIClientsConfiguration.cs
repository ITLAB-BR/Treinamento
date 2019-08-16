using ITLab.Treinamento.Api.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations.Security
{
    public class APIClientsConfiguration : EntityTypeConfiguration<APIClients>
    {
        public APIClientsConfiguration()
        {
            ToTable(nameof(APIClients), "AppSecurity");

            HasKey(k => k.Id);
            Property(p => p.Id).HasColumnName("APIClientId")
                .IsRequired()
                .HasMaxLength(25);

            Property(p => p.Type).IsRequired();
            Property(p => p.RefreshTokenLifeTimeInMinutes).IsRequired();
            Property(p => p.AllowedOrigin)
                .IsRequired()
                .HasMaxLength(100);

            HasMany(t => t.APIClientRefreshTokens)
                .WithRequired()
                .Map(t => t.MapKey("APIClientId").ToTable(nameof(APIClientRefreshToken), "AppSecurity"));
        }
    }
}