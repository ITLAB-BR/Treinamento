using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities.Security
{
    public class PreviousUserPasswords : IAuditable
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
    }
}