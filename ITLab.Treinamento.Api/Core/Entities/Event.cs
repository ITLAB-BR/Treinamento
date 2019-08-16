using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Core.Entities
{
    public class Event : IAuditable
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public string Color { get; set; }
        public bool AllDay { get; set; }


        public DateTime? ChangeDate { get; set; }
        public string ChangeUser { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationUser { get; set; }
    }
}