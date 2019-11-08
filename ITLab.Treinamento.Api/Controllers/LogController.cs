using ITLab.Treinamento.Api.Core.Configuration;
using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Text.RegularExpressions;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using ITLab.Treinamento.Api.Models;
using System.IO;
using System.Net;

namespace ITLab.Treinamento.Api.Controllers
{
    public class LogController : BaseController
    {
        private static class ResponseMessages
        {
            public const string file_not_found = "alerts:warning.log_not_found";
        }
        public JsonResult Get(DateTime? date)
        {
            if (!date.HasValue) date = DateTime.Today;

            var listLog = GetListLog(date.Value);

            if (listLog == null)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { message = ResponseMessages.file_not_found }, JsonRequestBehavior.AllowGet);
            }

            return Json(listLog, JsonRequestBehavior.AllowGet);
        }

        public static List<RowLog> GetListLog(DateTime date)
        {
            var dataRegex = @"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2},\d{3})";
            var dateString = date.ToString("yyyy/MM/dd");

            var fullPath = ((FileAppender)LogManager.GetRepository().GetAppenders()[0]).File;

            var path = new FileInfo(fullPath).Directory.Parent.Parent.FullName;

            var pathFile = string.Format("{0}/{1}.txt", path, dateString);
            if (!System.IO.File.Exists(pathFile))
                return null;

            var text = System.IO.File.ReadAllText(pathFile, Encoding.GetEncoding(1252));

            var skip = (text.StartsWith("\r\n")) ? 1 : 0;

            var split = Regex.Split(text, dataRegex).Skip(skip).ToArray();
            var listRow = new List<RowLog>();

            for (var i = 1; i < split.Count(); i += 2)
            {
                var data = split[i - 1];
                var log = split[i].Trim();
                listRow.Add(new RowLog { Date = data, Log = log });
            }
            listRow.Reverse();
            return listRow;
        }

        public class RowLog
        {
            public string Date { get; set; }
            public string Log { get; set; }
        }
    }
}