using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Models
{
    public class EventViewModel
    {
        public int? Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }
        public string Color { get; set; }
        public bool AllDay { get; set; }
    }
    public class EventFilterViewModel
    {
        public int? id { get; set; }
        public DateTime? since { get; set; }
        public DateTime? until { get; set; }
    }
}