(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('accountEditService', service);

    service.$inject = ['$http'];
    function service($http) {
        var service = {
            get: get,
            set: set
        };
        return service;

        function get() {
            return $http({
                url: '/api/User/GetMyAccount',
                method: 'GET'
            });
        }

        function set(request) {
            return $http({
                url: '/api/User/UpdateMyAccountAsync',
                method: 'PUT',
                data: request,
                headers: {
                    'Accept': 'application/json'
                }
            });
        }
    }

})();