using ITLab.Treinamento.Api.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Configurations
{
    public class NotificationUserConfiguration : EntityTypeConfiguration<NotificationUser>
    {
        public NotificationUserConfiguration()
        {
            ToTable("NotificationUsers");

            HasKey(t => new { t.NotificationId, t.UserId });

            HasRequired(t => t.User)
                .WithMany(t => t.Notifications)
                .HasForeignKey(t => t.UserId);

            HasRequired(t => t.Notification)
                .WithMany(t => t.Users)
                .HasForeignKey(t => t.NotificationId);
        }
    }
}