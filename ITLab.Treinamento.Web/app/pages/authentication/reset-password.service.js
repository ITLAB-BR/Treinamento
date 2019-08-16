(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('resetPasswordService', Service);

    Service.$inject = ['$http'];
    function Service($http) {
        var service = {
            resetPassword: _ResetPassword,

            getUser: _GetUser
        };

        return service;

        function _ResetPassword(request) {
            return $http({
                url: '/api/Password/ResetPasswordAsync',
                method: 'POST',
                data: request
            });
        };
        function _GetUser(userId) {
            return $http({
                url: '/api/User/GetEmail/' + userId,
                method: 'GET'
            });
        };
    };
})();