using ITLab.Treinamento.Api.Core.Cache;
using ITLab.Treinamento.Api.Core.Entities;
using ITLab.Treinamento.Api.Core.Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ITLab.Treinamento.Api.Core.Configuration
{
    public class SettingHelper
    {
        private static CacheWrapper CacheWrapper = new CacheWrapper();
        private const string CacheKeyName = "settings";
        private const double CacheExpirationInMinutes = 60;

        public static GeneralSettingsApp Get(bool refresh = false)
        {
            var settings = GetFromCache();
            if (settings == null || refresh == true)
            {
                RefreshCache();
                settings = GetFromCache();
            }

            return (GeneralSettingsApp)settings;
        }

        private static GeneralSettingsApp GetFromSource()
        {
            using (var context = new AppDbContext())
            {
                var settingsFromDataBase = context.GeneralSettings.ToList();
                return ParsetDataBaseToSettingsObject(settingsFromDataBase);
            }
        }

        public static void RefreshCache()
        {
            var settings = GetFromSource();
            AddInCache(settings);
        }

        private static GeneralSettingsApp GetFromCache()
        {
            return (GeneralSettingsApp)CacheWrapper.GetCachedItem(CacheKeyName);
        }

        public static void AddInCache(GeneralSettingsApp setting)
        {
            double secondsInOneMinute = 60;

            CacheWrapper.AddToMyCache(CacheKeyName, setting, CachePriority.Default, secondsInOneMinute * CacheExpirationInMinutes);
        }

        private static GeneralSettingsApp ParsetDataBaseToSettingsObject(IEnumerable<GeneralSettings> generalSettings)
        {
            var settings = new GeneralSettingsApp();

            foreach (var prop in settings.GetType().GetProperties())
            {
                var item = generalSettings.FirstOrDefault(s => s.SettingName == prop.Name);
                if (item == null) continue;

                object settingsValue;
                if (item.ValueBool != null) settingsValue = item.ValueBool;
                else if (item.ValueInt != null) settingsValue = item.ValueInt;
                else if (item.ValueString != null) settingsValue = item.ValueString;
                else settingsValue = null;
                //    throw new Exception(String.Format("Setting type data base is not recognized {0}", prop.Name));

                prop.SetValue(settings, settingsValue);
            }


            return settings;
        }

        public static IEnumerable<GeneralSettings> ParseObjectToSettingDataBase(GeneralSettingsApp settings)
        {
            var result = new List<GeneralSettings>();

            foreach (var prop in settings.GetType().GetProperties())
            {
                var setting = new GeneralSettings { SettingName = prop.Name };

                if (prop.PropertyType.Name == typeof(bool).Name)
                    setting.ValueBool = (bool)prop.GetValue(settings);
                else if ((prop.PropertyType.Name == typeof(int).Name) || (prop.PropertyType.IsEnum == true))
                    setting.ValueInt = (int)prop.GetValue(settings);
                else if (prop.PropertyType.Name == typeof(string).Name)
                    setting.ValueString = (string)prop.GetValue(settings);
                else
                    throw new Exception(String.Format("Setting type object is not recognized - Property Name: {0} / Property Type: {1}", prop.Name, prop.PropertyType.Name));

                result.Add(setting);
            }

            return result;
        }
    }
}