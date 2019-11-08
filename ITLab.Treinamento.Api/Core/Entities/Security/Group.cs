using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities.Security
{
    public class Group : IAuditable
    {
        //public Group(string name)
        //{
        //    this.Name = name;
        //}

        public Group()
        {
            Roles = new List<Role>();
            Users = new List<User>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
    }
}