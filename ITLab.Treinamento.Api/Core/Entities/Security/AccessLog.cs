using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities.Security
{
    public class AccessLog
    {
        public int Id { get; set; }
        public DateTime AttempAccessDateTime { get; set; }
        public string ClientIP { get; set; }
        public string Login { get; set; }
        public bool Active { get; set; }
        public string MessageCode { get; set; }
        public string MessageDescription { get; set; }
    }
}