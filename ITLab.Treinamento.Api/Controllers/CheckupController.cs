using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using ITLab.Treinamento.Api.Core.Infrastructure.Mail;
using ITLab.Treinamento.Api.Core.Upload;
using ITLab.Treinamento.Api.Models;
using log4net;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ITLab.Treinamento.Api.Controllers
{
    public class CheckupController : BaseController
    {
        public JsonResult GetApiHeartBeat()
        {
            const bool heartBeat = true;
            return Json(heartBeat, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLogInfo()
        {
            var appenders = LogManager.GetRepository().GetAppenders();

            var logInfoColletion = new List<LogInfoViewModel>();

            foreach (var item in appenders)
            {
                var logInfo = new LogInfoViewModel
                {
                    LogAppenderType = item.GetType().Name
                };
                if (item.GetType() == typeof(RollingFileAppender))
                {
                    logInfo.LogFile = true;
                    logInfo.FilePath = ((RollingFileAppender)item).File;
                }
                else
                {
                    logInfo.LogFile = false;
                }

                logInfoColletion.Add(logInfo);
            }

            return Json(logInfoColletion, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataBaseInfo()
        {
            var dataBaseInfo = new DataBaseInfoViewModel();

            using (var appDbContext = new AppDbContext())
            {
                dataBaseInfo.DataBaseName = appDbContext.Database.Connection.Database;
                dataBaseInfo.ServerInstanceName = appDbContext.Database.Connection.DataSource;
            }

            return Json(dataBaseInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDataBaseHeartBeat()
        {
            bool heartBeat;

            try
            {
                using (var appDbContext = new AppDbContext())
                {
                    heartBeat = appDbContext.Database.SqlQuery<int>("SELECT 1").First() > 0;
                }
            }
            catch (Exception)
            {
                heartBeat = false;
            }

            return Json(heartBeat, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSMTPInfo()
        {
            var eMailService = new EMailService();

            var smtpClientConfig = eMailService.SmtpClient;
            var smtpMailAddressConfig = eMailService.MailMessage;

            var smtpInfo = new SMTPInfoViewModel();

            if (smtpClientConfig.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                smtpInfo.DeliveryMethod = "Directory";
                smtpInfo.Directory = smtpClientConfig.PickupDirectoryLocation;
            }
            else
            {
                smtpInfo.DeliveryMethod = "SMTP Server";
                smtpInfo.Host = smtpClientConfig.Host;
                smtpInfo.Port = smtpClientConfig.Port;
                smtpInfo.EnableSsl = smtpClientConfig.EnableSsl;
                smtpInfo.CredentialUsername = ((NetworkCredential)smtpClientConfig.Credentials).UserName;
            }

            smtpInfo.DefaultFromAddress = smtpMailAddressConfig.From.Address != null ? smtpMailAddressConfig.From.Address : string.Empty;

            return Json(smtpInfo, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFileUploadInfo()
        {
            var uploadInfoViewModel = new UploadInfoViewModel
            {
                DirectoryTempUpload = UploadHelper.GetDirectoryTempPath(),
                MaxRequestLengthInBytes = UploadHelper.GetMaxRequestLengthInBytes(),
                FileCount = UploadHelper.GetFileCountInUploadDirectoryTemp(),
                FilesLengthInBytes = UploadHelper.GetFilesInUploadDirectoryTemp().Sum(c => c.Length)
            };

            return Json(uploadInfoViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestSMTP(string destinationAddress)
        {
            var resultTest = false;

            var eMailService = new EMailService();
            try
            {
                eMailService.SendAsync("Teste de envio de e-mail", "Teste", destinationAddress);
                resultTest = true;
            }
            catch (Exception)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                resultTest = false;
            }

            return Json(resultTest, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestLogDirectory()
        {
            var resultTest = false;

            var logInfoJson = new JavaScriptSerializer().Serialize(GetLogInfo().Data);
            var logInfo = new JavaScriptSerializer().Deserialize<List<LogInfoViewModel>>(logInfoJson);

            Logger.Error("Teste de gravação de log!");
            Logger.Info("Teste de gravação de log!");
            Logger.Warn("Teste de gravação de log!");

            foreach (var item in logInfo.Where(x => x.LogFile))
            {
                if (System.IO.File.Exists(item.FilePath))
                    resultTest = true;
                else
                {
                    resultTest = false;
                    break;
                }
            }

            return Json(resultTest, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TestUploadDirectory()
        {
            var resultTest = false;
            try
            {
                UploadHelper.GetDirectoryTempPathOrCreateIfNotExists();
                resultTest = true;
            }
            catch (Exception)
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                resultTest = false;
            }
            return Json(resultTest, JsonRequestBehavior.AllowGet);
        }
    }
}