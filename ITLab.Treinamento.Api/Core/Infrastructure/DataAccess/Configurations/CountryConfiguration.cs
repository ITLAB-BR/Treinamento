using ITLab.Treinamento.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations
{
    public class CountryConfiguration : EntityTypeConfiguration<Country>
    {
        public CountryConfiguration()
        {
            ToTable(nameof(Country));

            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("CountryId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(t => t.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(30)
                .HasColumnAnnotation(
                    IndexAnnotation.AnnotationName, 
                    new IndexAnnotation(
                        new IndexAttribute("IX_Country_Name") { IsUnique = true }));

            Property(t => t.Active)
                .IsRequired();

            HasMany(t => t.Users)
                .WithMany(t => t.Countries)
                .Map(t => t.MapLeftKey("CountryId")
                           .MapRightKey("UserId")
                           .ToTable("UsersCountry"));
        }
    }
}