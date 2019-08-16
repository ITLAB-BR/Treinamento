using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;
using log4net;
using log4net.Core;
using Microsoft.AspNet.Identity;

namespace ITLab.Treinamento.Api.ActionFilters
{
    public sealed class LogUncaughtExceptionApiFilterAttribute : ExceptionFilterAttribute
    {
        [ExcludeFromCodeCoverage]
        public ILog Logger { get; set; }

        public override bool AllowMultiple => false;

        public LogUncaughtExceptionApiFilterAttribute()
        {
            Logger = LogManager.GetLogger(GetType());
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            var exception = context.Exception;
            if (exception == null) return;

            var userName = String.IsNullOrEmpty(HttpContext.Current.User.Identity.GetUserName())
                                ? "anonymous"
                                : HttpContext.Current.User.Identity.GetUserName();

            var complement = $"[Username: {userName} | Controller: { context.Request.GetRouteData().Values["Controller"]} | Action: { context.Request.GetRouteData().Values["Action"]}]";

            Logger.Fatal(complement, exception);

            var exceptionhttpException = (HttpException)exception;
            var statusCode = (HttpStatusCode)exceptionhttpException.GetHttpCode();

            context.Response = new HttpResponseMessage(statusCode);
        }
    }
}