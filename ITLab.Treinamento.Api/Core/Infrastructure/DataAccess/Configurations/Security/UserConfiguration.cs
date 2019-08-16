using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Infrastructure.Annotations;
using ITLab.Treinamento.Api.Core.Entities.Security;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Security.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable(nameof(User), "AppSecurity");

            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("UserId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name).HasMaxLength(70).IsRequired();
            Property(t => t.UserName).HasMaxLength(25)
                                        .HasColumnAnnotation(
                                            IndexAnnotation.AnnotationName,
                                            new IndexAnnotation(
                                                new IndexAttribute("IX_User_UserName") { IsUnique = true }));

            Property(t => t.Email).HasMaxLength(70).IsRequired();
            Property(t => t.PhoneNumber).HasMaxLength(15);
            Property(t => t.Active).IsRequired();
            Property(t => t.AuthenticationType).IsRequired();
            Property(t => t.AccessAllDataVisibility).IsRequired();
            Property(t => t.LastPasswordChangedDate).IsOptional();

            HasMany(t => t.Roles)
                .WithRequired(t => t.User)
                .HasForeignKey(t => t.UserId);

            HasMany(c => c.Logins).WithOptional().HasForeignKey(c => c.UserId);
            HasMany(c => c.Claims).WithOptional().HasForeignKey(c => c.UserId);

            HasMany(u => u.PreviousUserPasswords).WithOptional().HasForeignKey(c => c.UserId);

            HasOptional(us => us.UserPhoto)
                .WithRequired(u => u.User);
        }
    }
}