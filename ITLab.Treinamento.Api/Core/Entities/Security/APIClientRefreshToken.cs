using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities.Security
{
    public class APIClientRefreshToken : IAuditable
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public DateTime IssuedUTC { get; set; }
        public DateTime ExpiresUTC { get; set; }
        public string ProtectedTicket { get; set; }
        
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
    }
}