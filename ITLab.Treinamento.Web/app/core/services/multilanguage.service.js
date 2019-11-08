(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('multilanguage', Service);

    Service.$inject = ['$cookies', 'generalConfig'];

    function Service($cookies, generalConfig) {
        var service = {
            currentLanguage: _GetCurrentLanguage(),
            setLanguage: _SetLanguage,
            getFormatShortDate: _GetFormatShortDate
        };

        return service;

        function _SetLanguage(language) {
            language = language || service.currentLanguage;

            service.currentLanguage = language;
            i18n.setLng(language, function () { $('body').i18n(); });
            moment.locale(language);
            numeral.language(language.toLowerCase());
        };

        function _GetCurrentLanguage() {
            return i18n.lng() || $cookies.get('i18next') || generalConfig.language.languageDefault;
        };

        // TODO: definir um lugar para deixar essa função
        function _GetFormatShortDate() {
            return moment.localeData()._longDateFormat.L.replace(/D/g, 'd').replace(/Y/g, 'y');
        }
    };
})();