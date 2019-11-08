using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ITLab.Treinamento.Api.Core.Entities.Security;
using System.Data.Entity.Infrastructure.Annotations;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Security.Configurations
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            ToTable("Roles", "AppSecurity");
            HasKey(t => t.Id);

            Property(t => t.Id).HasColumnName("RoleId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(t => t.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(50)
                .HasColumnAnnotation(
                        IndexAnnotation.AnnotationName,
                        new IndexAnnotation(
                            new IndexAttribute("IX_RoleName") { IsUnique = true }));

            HasMany(t => t.Users)
                .WithRequired(t => t.Role)
                .HasForeignKey(t => t.RoleId);

            //HasMany(t => t.Permissions)
            //    .WithMany(t => t.Roles)
            //    .Map(u =>
            //    {
            //        u.MapLeftKey("RoleId");
            //        u.MapRightKey("PermissionId");
            //        u.ToTable("RolesPermissions", "Security");
            //    });
        }
    }
}