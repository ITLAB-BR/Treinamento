(function () {
    angular.module('itlabtreinamento').filter('notification', filter);
    filter.$inject = ['generalConfig'];
    function filter(generalConfig) {
        return function (text) {
            const result = text.replace(/{{url.*?}}(\S+)/, '<a href="' + generalConfig.baseAddressApi + '$1">' + generalConfig.baseAddressApi + '$1 </a>')
                                .replace(/{{(.+?)}}/g, function (_, key) { i18n.t(key) });

            return result;
        };
    }
})();
