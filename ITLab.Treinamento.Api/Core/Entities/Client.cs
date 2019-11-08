using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities
{
    public class Client : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public Int64? CNPJ { get; set; }
        public Int64? CPF { get; set; }
        public Int64? Telephone { get; set; }
        public DateTime? BirthDate { get; set; }
        public bool Active { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
    }
}