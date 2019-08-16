using System.Data.Entity.ModelConfiguration;
using ITLab.Treinamento.Api.Core.Entities.Security;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Security.Configurations
{
    public class UserRoleConfiguration : EntityTypeConfiguration<UserRole>
    {
        public UserRoleConfiguration()
        {
            ToTable("UsersRoles", "AppSecurity");

            HasKey(t => new { t.UserId, t.RoleId });

            Property(t => t.UserId).HasColumnName("UserId");
            Property(t => t.RoleId).HasColumnName("RoleId");
        }
    }
}