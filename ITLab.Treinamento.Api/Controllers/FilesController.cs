using ITLab.Treinamento.Api.Core.Upload;
using System.IO;
using System.Net.Mime;
using System.Web.Mvc;

namespace ITLab.Treinamento.Api.Controllers
{
    public class FilesController : BaseController
    {
        public FileResult Index(string fileName)
        {
            var path = Path.Combine(UploadHelper.GetDirectoryTempPathOrCreateIfNotExists(), fileName);
            return File(path, MediaTypeNames.Text.Plain, fileName);
        }
    }
}