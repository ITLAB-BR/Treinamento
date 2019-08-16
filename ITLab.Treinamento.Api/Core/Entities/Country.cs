using ITLab.Treinamento.Api.Core.Entities.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITLab.Treinamento.Api.Core.Entities
{
    /// <summary>
    /// Classe que representa um grupo ou unidade de negócio abordados pelo sistema 
    /// (Ex.: Planta, Área, Departamento)
    /// </summary>
    public class Country : IAuditable
    {
        public Country()
        {
            Users = new List<User>();
        }
        public byte Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
    }
}
