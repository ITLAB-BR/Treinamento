using ITLab.Treinamento.Api.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations.Security
{
    public class UserPhotoConfiguration : EntityTypeConfiguration<UserPhoto>
    {
        public UserPhotoConfiguration()
        {
            ToTable(nameof(UserPhoto), "AppSecurity");
            HasKey(t => t.UserId);

            Property(t => t.Photo).IsRequired();
        }
    }
}