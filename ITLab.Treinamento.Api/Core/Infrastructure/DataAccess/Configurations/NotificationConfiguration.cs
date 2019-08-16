using ITLab.Treinamento.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations
{
    public class NotificationConfiguration : EntityTypeConfiguration<Notification>
    {
        public NotificationConfiguration()
        {
            ToTable("Notifications");

            HasKey(t => t.Id);
            Property(t => t.Id).HasColumnName("NotificationId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity) ;

            Property(t => t.Message)
                .HasColumnName("Message")
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}