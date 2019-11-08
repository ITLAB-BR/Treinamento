using ITLab.Treinamento.Api.Core.Entities.Security;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations.Security
{
    public class AccessLogConfiguration : EntityTypeConfiguration<AccessLog>
    {
        public AccessLogConfiguration()
        {
            ToTable(nameof(AccessLog), "AppSecurity");
            HasKey(t => t.Id);

            Property(t => t.Id).HasColumnName("AccessLogId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.AttempAccessDateTime)
                .IsRequired()
                .HasColumnName("AttempAccessDateTime")
                .HasColumnAnnotation(
                        IndexAnnotation.AnnotationName,
                        new IndexAnnotation(
                            new IndexAttribute("IX_AccessLog_AttempAccessDateTime")));

            Property(t => t.ClientIP)
                .IsRequired()
                .HasColumnName("ClientIP")
                .HasMaxLength(15);

            Property(t => t.Login)
                .IsRequired()
                .HasColumnName("Login")
                .HasMaxLength(70);

            Property(t => t.Active)
                .IsRequired();
        }
    }
}