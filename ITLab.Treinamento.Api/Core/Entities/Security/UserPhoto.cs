using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities.Security
{
    public class UserPhoto
    {
        public int UserId { get; set; }
        public byte[] Photo { get; set; }
        public virtual User User { get; set; }
    }
}