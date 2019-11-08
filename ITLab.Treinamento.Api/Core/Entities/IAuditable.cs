using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities
{
    /// <summary>
    /// As entidades que implementam essa interface tem os campos auditoria
    /// preenchidos automaticamente ao ser persistida
    /// </summary>
    public interface IAuditable
    {
        DateTime CreationDate { get; set; }
        string CreationUser { get; set; }

        DateTime? ChangeDate { get; set; }
        string ChangeUser { get; set; }
    }
}