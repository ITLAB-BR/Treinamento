using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ITLab.Treinamento.Api.Core.Entities.Security
{
    public class Role : IdentityRole<int, UserRole>, IAuditable
    {
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public virtual ICollection<Group> Groups { get; set; }
    }
}
