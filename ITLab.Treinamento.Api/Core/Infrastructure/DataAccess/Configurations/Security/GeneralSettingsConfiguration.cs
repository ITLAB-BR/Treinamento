using ITLab.Treinamento.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations.Security
{
    public class GeneralSettingsConfiguration : EntityTypeConfiguration<GeneralSettings>
    {
        public GeneralSettingsConfiguration()
        {
            ToTable(nameof(GeneralSettings));

            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("GeneralSettingId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.SettingName).IsRequired().HasMaxLength(50)
                                            .HasColumnAnnotation(
                                            IndexAnnotation.AnnotationName,
                                            new IndexAnnotation(
                                                new IndexAttribute("IX_GeneralSetting_SettingName") { IsUnique = true }));
        }
    }
}