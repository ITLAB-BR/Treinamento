using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ITLab.Treinamento.Api.Models
{
    public class UploadInfoViewModel
    {
        public string DirectoryTempUpload { get; set; }
        public int MaxRequestLengthInBytes { get; set; }
        public long FileCount { get; set; }
        public long FilesLengthInBytes { get; set; }
    }

    public class UploadFile
    {
        public long LenghtInBytes { get; set; }
        public string Name { get; set; }
    }

}