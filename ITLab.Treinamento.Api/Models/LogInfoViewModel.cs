using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Models
{
    public class LogInfoViewModel
    {
        public string LogAppenderType { get; set; }
        public bool LogFile { get; set; }
        public string FilePath { get; set; }
    }
}