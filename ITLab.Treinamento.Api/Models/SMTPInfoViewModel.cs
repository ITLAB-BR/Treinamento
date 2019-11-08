using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Models
{
    public class SMTPInfoViewModel
    {
        public string DeliveryMethod { get; set; }
        public string Directory { get; set; }
        public string DefaultFromAddress { get; set; }
        public string Host { get; set; }
        public int? Port { get; set; }
        public bool EnableSsl { get; set; }
        public string CredentialUsername { get; set; }
    }
}