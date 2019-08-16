(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('logService', Service);

    Service.$inject = ['$http'];
    function Service($http) {
        var service = {
            get: _Get
        };
        return service;

        function _Get(request) {
            return $http({
                url: '/api/Log/Get',
                params: request,
                method: 'GET'
            });
        };
    };
})();