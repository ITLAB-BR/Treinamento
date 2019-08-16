using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Infrastructure.DataAccess.Conventions
{
    /// <summary>
    /// Convention que configura o mapeamento dos campos de auditoria
    /// </summary>
    public class AuditableConvention : Convention
    {
        public AuditableConvention()
        {
            Types().Where(t => t.GetInterfaces().Any(i => i == typeof(IAuditable)))
                   .Configure(t =>
                   {
                       t.Property("CreationDate").HasColumnName("CreationDate").IsRequired();   //Username do usuário
                       t.Property("CreationUser").HasColumnName("CreationUser").HasMaxLength(25).IsRequired();

                       t.Property("ChangeDate").HasColumnName("ChangeDate");
                       t.Property("ChangeUser").HasColumnName("ChangeUser").HasMaxLength(25);   //Username do usuário
                   });
        }
    }
}