using ITLab.Treinamento.Api.IdentityConfig;
using ITLab.Treinamento.Api.Core.Security.Authorization;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using Microsoft.AspNet.Identity;
using log4net;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using log4net.Core;
using ITLab.Treinamento.Api.Core.Entities.Security;
using ITLab.Treinamento.Api.Core.Entities;
using System.Data.Entity;

namespace ITLab.Treinamento.Api.Controllers
{
    public abstract class BaseController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        [ExcludeFromCodeCoverage]
        public ILog Logger { get; set; }

        protected BaseController()
        {
            Logger = LogManager.GetLogger(GetType());
        }

        protected BaseController(ApplicationUserManager userManager)
        {
            if (!ApplicationUserManager.AuthenticateTypeIsSet())
            {
                throw new Exception("Authenticate type is not set.");
            }

            _userManager = userManager;
        }

        protected ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        protected ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        protected string UserLoggedUserName
        {
            get
            {
                return HttpContext.User.Identity.GetUserName();
            }
        }

        protected int UserLoggedId
        {
            get
            {
                return Convert.ToInt32(HttpContext.User.Identity.GetUserId());
            }
        }

        protected static bool HasPermission(Permissions permission)
        {
            using (var context = new AppDbContext())
            {
                return context.Users.HasPermission(permission);
            }
        }

        /// <summary>
        /// Gera uma notificação para o usuário logado
        /// </summary>
        /// <param name="message">Mensagem da notificação</param>
        protected static void NotifyCurrentUser(string message)
        {
            using (var context = new AppDbContext())
            {
                var userid = AppDbContext.GetCurrentUserId();
                var user = new User { Id = userid };

                context.Users.Attach(user);

                user.Notify(message);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Gera uma notificação para todos os usuários ativos de uma determinado grupo
        /// </summary>
        /// <param name="groupId">Id do grupo</param>
        /// <param name="message">Mensagem da notificação</param>
        protected static void NotifiyActiveUsersByGroup(int groupId, string message)
        {
            using (var context = new AppDbContext())
            {
                var userIds = context.Groups.Include(r => r.Users)
                                            .Where(r => r.Id == groupId)
                                            .SelectMany(r => r.Users)
                                            .Where(u => u.Active)
                                            .Select(u => u.Id)
                                            .ToArray();

                Notify(userIds, message, context);
            }
        }

        /// <summary>
        /// Gera uma notificação para todos os usuários ativos do sistema
        /// </summary>
        /// <param name="message">Mensagem da notificação</param>
        protected static void NotifyAllActiveUsers(string message)
        {
            using (var context = new AppDbContext())
            {
                var userIds = context.Users.Where(u => u.Active)
                                           .Select(u => u.Id)
                                           .ToArray();

                Notify(userIds, message, context);
            }

        }

        private static void Notify(int[] userIds, string message, AppDbContext context)
        {
            var users = userIds.Select(u =>
                                          {
                                              var user = new User { Id = u };
                                              context.Users.Attach(user);
                                              return user;
                                          });

            var notification = new Notification { Message = message };
            notification.To(users);

            context.Set<Notification>().Add(notification);
            context.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Trata as mensagens provindas no result.Errors para melhor serem lidas e tratadas pelo Front End
        /// </summary>
        /// <param name="identityResult">IdentityResult (não tratado)</param>
        /// <returns>IdentityResult (tratado)</returns>
        protected static IdentityResult SanitizeResult(IdentityResult identityResult)
        {
            IdentityResult result;

            if (identityResult.Succeeded)
                result = IdentityResult.Success;
            else
            {
                var resultToReturn = new List<string>();
                foreach (var resultError in identityResult.Errors)
                {
                    var errors = resultError.ToString().Split('.').Where(x => !String.IsNullOrEmpty(x)).ToArray();
                    foreach (var error in errors)
                    {
                        var errorSanitized = String.Concat("alerts:error.", error.Trim().Replace(" ", "_").Replace(".", "").ToLower());
                        resultToReturn.Add(errorSanitized);
                    }
                }
                result = new IdentityResult(resultToReturn);
            }

            return result;
        }
        /// <summary>
        /// Seta o HttpContext.Response.StatusCode com base no IdentityResult gerado
        /// </summary>
        /// <param name="result">IdentityResult</param>
        protected void SetHttpContextResponseStatusCode(IdentityResult result)
        {
            if (result == null)
                HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            else if (!result.Succeeded)
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        }

        /// <summary>
        /// Seta o HttpContext.Response.StatusCode como BadRequest para um ModelState inválido
        /// e agrupa as mensagens de erro em um objeto message serializado
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns>string[] message</returns>
        protected JsonResult BadRequest(ModelStateDictionary modelState)
        {
            if (modelState.IsValid)
                return null;

            HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errorMessage = new List<string>();

            foreach (ModelState field in modelState.Values)
                foreach (ModelError error in field.Errors)
                {
                    errorMessage.Add(error.ErrorMessage);
                }

            return Json(new { message = errorMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}