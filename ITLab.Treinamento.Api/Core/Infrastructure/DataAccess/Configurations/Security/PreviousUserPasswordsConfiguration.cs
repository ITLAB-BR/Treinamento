using ITLab.Treinamento.Api.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations.Security
{
    public class PreviousUserPasswordsConfiguration : EntityTypeConfiguration<PreviousUserPasswords>
    {
        public PreviousUserPasswordsConfiguration()
        {
            ToTable(nameof(PreviousUserPasswords), "AppSecurity");
            HasKey(t=>t.Id);

            Property(t => t.PasswordHash).IsRequired();

            Property(t => t.Id).HasColumnName("PreviousUserPasswordsId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}