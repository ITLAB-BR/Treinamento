using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Policy.ViolationHandlers;
using log4net;
using Microsoft.AspNet.Identity;

namespace ITLab.Treinamento.Api.ActionFilters
{
    public class ErrorResult
    {
        public string UserName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string ExceptionStackTrace { get; set; }
        public string ExceptionSource { get; set; }
        public string InnerException { get; set; }
    }

    public sealed class LogUncaughtExceptionMvcFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public ILog Logger { get; set; }
        private ErrorResult errorResult;

        public LogUncaughtExceptionMvcFilterAttribute()
        {
            Logger = LogManager.GetLogger(GetType());
        }

        public void OnException(ExceptionContext filterContext)
        {
            var statusCode = HttpStatusCode.InternalServerError;

            var userName = String.IsNullOrEmpty(HttpContext.Current.User.Identity.GetUserName())
                                ? "anonymous"
                                : HttpContext.Current.User.Identity.GetUserName();

            var controllerName = (string)filterContext.RouteData.Values["controller"];
            var actionName = (string)filterContext.RouteData.Values["action"];
            var complement = $"[Username: {userName} | Controller: { controllerName } | Action: { actionName }]";

            Logger.Fatal(complement, filterContext.Exception);

            errorResult = new ErrorResult
            {
                UserName = userName,
                ControllerName = controllerName,
                ActionName = actionName
            };

            if (filterContext.Exception is FluentSecurity.PolicyViolationException)
            {
                statusCode = HttpStatusCode.Unauthorized;
            }
            else if (filterContext.Exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Forbidden;
            }
            else
            {
                errorResult.ExceptionStackTrace = filterContext.Exception.StackTrace;
                errorResult.ExceptionSource = filterContext.Exception.Source;
                errorResult.InnerException = filterContext.Exception.InnerException != null ? filterContext.Exception.InnerException.ToString() : null;
            }

            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.StatusCode = (int)statusCode;
            filterContext.Result = new JsonResult
            {
                Data = errorResult,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}