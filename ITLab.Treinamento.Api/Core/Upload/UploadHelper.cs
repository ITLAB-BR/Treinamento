using ITLab.Treinamento.Api.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Configuration;

namespace ITLab.Treinamento.Api.Core.Upload
{
    public static class UploadHelper
    {
        public static List<string> Save(this HttpFileCollectionBase source)
        {
            var uploadedFiles = new List<string>();

            if (source.Count == 0)
                return uploadedFiles;

            for (int i = 0; i < source.Count; i++)
            {
                var fileContent = source[i];
                if (!(fileContent != null && fileContent.ContentLength > 0))
                    continue;

                var stream = fileContent.InputStream;
                var fileName = Path.GetFileName(fileContent.FileName);
                var pathFile = Path.Combine(GetDirectoryTempPathOrCreateIfNotExists(), fileName);

                using (var fileStream = File.Create(pathFile))
                {
                    stream.Position = 0;
                    stream.CopyTo(fileStream);
                }
                uploadedFiles.Add(pathFile);
            }

            return uploadedFiles;
        }

        public static string GetDirectoryTempPathOrCreateIfNotExists()
        {
            if (!Directory.Exists(SettingHelper.Get().UploadDirectoryTemp))
                Directory.CreateDirectory(SettingHelper.Get().UploadDirectoryTemp);

            return SettingHelper.Get().UploadDirectoryTemp;
        }

        public static string GetDirectoryTempPath()
        {
            return SettingHelper.Get().UploadDirectoryTemp;
        }

        public static int GetMaxRequestLengthInBytes()
        {
            var config = WebConfigurationManager.OpenWebConfiguration("~");
            var section = config.GetSection("system.web/httpRuntime") as HttpRuntimeSection;
            return section.MaxRequestLength * 1024;
        }

        public static int GetFileCountInUploadDirectoryTemp()
        {
            return Directory.GetFiles(GetDirectoryTempPathOrCreateIfNotExists(), "*.*", SearchOption.AllDirectories).Length;
        }

        public static List<FileInfo> GetFilesInUploadDirectoryTemp()
        {
            var returnListFiles = new List<FileInfo>();

            var files = Directory.GetFiles(GetDirectoryTempPath(), "*.*", SearchOption.AllDirectories);

            foreach (string name in files)
            {
                returnListFiles.Add(new FileInfo(name));
            }
            return returnListFiles;
        }

        public static void DeleteFileFromDirectoryTemp(string pathFile)
        {
            if (!File.Exists(pathFile))
                throw new Exception("Arquivo não existe!!!");

            File.Delete(pathFile);
        }

        public static byte[] GetFileAsImage(string pathFile)
        {
            byte[] image;
            using (Bitmap imageBitmap = (Bitmap)Image.FromFile(pathFile))
            {
                image = ImageToByteArray(imageBitmap);
            }
            return image;
        }

        private static byte[] ImageToByteArray(Image image)
        {
            var imageMemoryStream = new MemoryStream();
            image.Save(imageMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return imageMemoryStream.ToArray();
        }
    }
}