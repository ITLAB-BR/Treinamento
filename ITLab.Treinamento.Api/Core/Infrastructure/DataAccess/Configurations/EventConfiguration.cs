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
    public class EventConfiguration : EntityTypeConfiguration<Event>
    {
        public EventConfiguration()
        {
            ToTable(nameof(Event));

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("EventId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Description).IsRequired().HasMaxLength(30);
            Property(x => x.Date).IsRequired().HasColumnType("DATE");
            Property(x => x.Start).HasPrecision(0);
            Property(x => x.End).HasPrecision(0);
            Property(x => x.Color).HasMaxLength(7);
        }
    }
}