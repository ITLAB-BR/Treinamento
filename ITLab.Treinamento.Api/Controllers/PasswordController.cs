using ITLab.Treinamento.Api.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ITLab.Treinamento.Api.Controllers
{
    public class PasswordController : SecurityController
    {
        [HttpPost]
        public async Task<JsonResult> ForgotPasswordAsync(ForgotPasswordViewModel model)
        {
            var user = await UserManager.FindByEmailAsync(model.Email);
            if (user == null)             //|| !(await UserManager.IsEmailConfirmedAsync(user.Id)))
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("user_notfound", JsonRequestBehavior.AllowGet);
            }

            var code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);

            var callbackUrl = CreateUrlSetNewPassword(model.FrontEndUrl, user.Id, code);

            //TODO: Implementar um template de e-mail
            await UserManager.SendEmailAsync(user.Id, "Reiniciar Senha", "Para reiniciar sua senha clique neste link: <a href=\"" + callbackUrl + "\">link</a>");

            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ResetPasswordAsync(ResetPasswordViewModel model)
        {
            var result = IsCorrectConfirmNewPassword(model.Password, model.ConfirmPassword);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                result = user == null ? new IdentityResult("user_notfound") : await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            }

            result = SanitizeResult(result);
            SetHttpContextResponseStatusCode(result);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeMyPasswordAsync(ChangePasswordViewModel model)
        {
            var result = await ConcretizeChangePasswordAsync(model, UserLoggedId);

            result = SanitizeResult(result);
            SetHttpContextResponseStatusCode(result);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> ChangeMyPasswordExpiredAsync(ChangePasswordViewModel model)
        {
            IdentityResult result;

            var user = await UserManager.FindUserAsync(model.Username);
            result = user == null ? new IdentityResult("user_not_found") : !user.IsPasswordExpired() ? new IdentityResult("password_is_not_expired") : await ConcretizeChangePasswordAsync(model, user.Id);

            result = SanitizeResult(result);
            SetHttpContextResponseStatusCode(result);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private async Task<IdentityResult> ConcretizeChangePasswordAsync(ChangePasswordViewModel model, int userId)
        {
            var result = IsCorrectConfirmNewPassword(model.NewPassword, model.ConfirmPassword);
            if (!result.Succeeded)
                return result;

            result = await UserManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);

            return result;
        }

        private static IdentityResult IsCorrectConfirmNewPassword(string newPassword, string confirmNewPassword)
        {
            IdentityResult result;

            result = newPassword != confirmNewPassword ? new IdentityResult("password_not_equals") : IdentityResult.Success;

            return result;
        }

        private static Uri CreateUrlSetNewPassword(string urlFrontEnd, int userId, string code)
        {
            var pageAndParameters = new StringBuilder("reset");
            pageAndParameters.Append("?userId=").Append(userId);
            pageAndParameters.Append("&code=").Append(HttpUtility.UrlEncode(code));

            var urlBase = urlFrontEnd.Replace("recover", pageAndParameters.ToString());
            Uri urlSetNewPassword;
            Uri.TryCreate(urlBase, UriKind.Absolute, out urlSetNewPassword);

            return urlSetNewPassword;
        }
    }
}