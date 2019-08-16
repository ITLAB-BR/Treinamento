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
    public class GroupConfiguration : EntityTypeConfiguration<Group>
    {
        public GroupConfiguration()
        {
            ToTable(nameof(Group), "AppSecurity");
            HasKey(t => t.Id);

            Property(t => t.Id).HasColumnName("GroupId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(30)
                .HasColumnAnnotation(
                        IndexAnnotation.AnnotationName,
                        new IndexAnnotation(
                            new IndexAttribute("IX_GroupName") { IsUnique = true }));

            HasMany(t => t.Roles)
                .WithMany(t => t.Groups)
                .Map(m =>
                {
                    m.MapLeftKey("GroupId");
                    m.MapRightKey("RoleId");
                    m.ToTable("GroupRole", "AppSecurity");
                });

            HasMany(t => t.Users)
                .WithMany(t => t.Group)
                .Map(m =>
                {
                    m.MapLeftKey("GroupId");
                    m.MapRightKey("UserId");
                    m.ToTable("GroupUser", "AppSecurity");
                });

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