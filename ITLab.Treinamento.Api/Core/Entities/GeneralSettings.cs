using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities
{
    public class GeneralSettings : IAuditable
    {
        public int Id { get; set; }
        public string SettingName { get; set; }
        public bool? ValueBool { get; set; }
        public int? ValueInt { get; set; }
        public string ValueString { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
    }
}