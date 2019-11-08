(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .factory('forgotPasswordService', Service);

    Service.$inject = ['$http', '$location'];
    function Service($http, $location) {
        var service = {
            resquestRecoveryPassword: _ResquestRecoveryPassword
        };
        return service;


        function _ResquestRecoveryPassword(email) {
            var request = {
                email: email,
                FrontEndUrl: $location.$$absUrl
            };
            return $http({
                url: '/api/Password/ForgotPasswordAsync',
                method: 'POST',
                data: request
            });
        };

    }
})();
