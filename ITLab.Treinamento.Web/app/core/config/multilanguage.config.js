/**
 * @by          IT Lab
 * @author      camilla.vianna
 * @desc        configuração para internacionalização com o i18next
 */

(function () {
    'use strict';

    angular.module('itlabtreinamento').run(multilanguageConfig)

    multilanguageConfig.$inject = ['$cookies', 'generalConfig', 'multilanguage'];
    function multilanguageConfig($cookies, generalConfig, multilanguage) {
        var currentLanguage = multilanguage.currentLanguage;

        $.i18n.init({
            lng: currentLanguage,
            resGetPath: 'public/locales/__lng__/__ns__.json',
            load: 'current',
            debug: false,
            fallbackLng: generalConfig.language.languageDefault,
            ns: {
                // namespaces = lista dos arquivos json que compõe cada língua
                namespaces: ['general', 'general-app', 'alerts', 'alerts-app', 'datatables'],
                defaultNs: 'general'
            },
            fallbackNS: 'general-app',
            getAsync: false,
            interpolation: { escapeValue: false }
        }, function (err, t) {
            if (!err) return;

            if (err[0].toLowerCase() == 'not found') {
                var generalizedLanguage = currentLanguage.split('-')[0];
                if (generalizedLanguage != currentLanguage)
                    multilanguage.setLanguage(generalizedLanguage);
                else
                    multilanguage.setLanguage(generalConfig.language.languageDefault);
            }
        });

        multilanguage.setLanguage();
    };
})();