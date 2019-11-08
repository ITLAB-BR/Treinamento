(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('apiClientService', Service);

    Service.$inject = ['$http'];
    function Service($http) {
        var service = {
            list: _List,
            post: _Post,
            put: _Put,
            putSecret: _PutSecret
        };
        return service;

        function _List() {
            return $http({
                url: '/api/ApiClient/Get/',
                method: 'get'
            });
        };
        function _Put(request) {
            return $http({
                url: '/api/ApiClient/Put',
                method: 'PUT',
                data: request
            });
        };
        function _Post(request) {
            return $http({
                url: '/api/ApiClient/Post',
                method: 'POST',
                data: request
            });
        };
        function _PutSecret(request) {
            return $http({
                url: '/api/APIClient/Secret/Put',
                method: 'PUT',
                data: request
            });
        };
    };
})();