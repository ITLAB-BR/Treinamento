(function () {
    'use strict';

    angular.module('itlabtreinamento')
           .factory('searchFactory', searchFactory)
           .factory('formFactory', formFactory);

    function searchFactory() {
        var factory = {
            getStatuses: getStatuses
        };

        return factory;

        function getStatuses() {
            return [
                { key: null, text: i18n.t('label.all') },
                { key: true, text: i18n.t('label.active') },
                { key: false, text: i18n.t('label.inactive') }
            ];
        };
    };

    function formFactory() {
        var factory = {
            deserialize: _Deserialize,
            numberfy: _Numberfy
        };

        return factory;

        function _Deserialize(value) {
            if (typeof value !== 'undefined')
                value = angular.fromJson(value);
        };
        function _Numberfy(value) {
            // remove a máscara de um número
            if (value && typeof value != "number")
                value = Number(value.replace(/\D/g, ''));

            return value;
        }
    };
})();