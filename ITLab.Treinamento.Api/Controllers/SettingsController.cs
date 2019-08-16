using ITLab.Treinamento.Api.Core.Configuration;
using ITLab.Treinamento.Api.Core.ExtensionsMethod;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using System.Linq;
using System.Web.Mvc;

namespace ITLab.Treinamento.Api.Controllers
{
    public class SettingsController : BaseController
    {
        public JsonResult Get(bool refresh = false)
        {
            var setting = SettingHelper.Get(refresh);

            HashSensitiveInformation(setting);
            
            return Json(setting, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLayoutSkin(bool refresh = false)
        {
            var setting = SettingHelper.Get(refresh);

            return Json(setting.LayoutSkin, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Set(GeneralSettingsApp setting)
        {
            var generalSettings = SettingHelper.ParseObjectToSettingDataBase(setting);

            using (var context = new AppDbContext())
            {
                var settingDataBase = context.GeneralSettings.ToArray();

                foreach (var item in generalSettings)
                {
                    if (item.SettingName == "SMTPCredentialsPassword")
                    {
                        var actualPassword = SettingHelper.Get(true).SMTPCredentialsPassword;

                        if (!UserChangedSMTPCredentialsPassword(item.ValueString, actualPassword))
                        {
                            continue;
                        }
                    }

                    var settingItem = settingDataBase.Single(x => x.SettingName == item.SettingName);
                    settingItem.ValueBool = item.ValueBool;
                    settingItem.ValueInt = item.ValueInt;
                    settingItem.ValueString = item.ValueString;
                }
                context.SaveChanges();
            }

            SettingHelper.RefreshCache();

            return Json(setting, JsonRequestBehavior.AllowGet);
        }

        private static void HashSensitiveInformation(GeneralSettingsApp settings)
        {
            settings.SMTPCredentialsPassword = settings.SMTPCredentialsPassword.GetHash();
        }

        private static bool UserChangedSMTPCredentialsPassword(string passwordInputedByUser, string actualPassword)
        {
            if (passwordInputedByUser == null || passwordInputedByUser.Trim().Length == 0)
            {
                return false;
            } else
            {
                return !(passwordInputedByUser == actualPassword.GetHash());
            }
        }
    }
}