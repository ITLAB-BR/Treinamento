(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('clientService', Service);

    Service.$inject = ['$http'];
    function Service($http) {
        var service = {
            list: _List,
            get: _Get,
            put: _Put,
            post: _Post,
            excel: _Excel
        };
        return service;

        function _List(request) {
            return $http({
                url: '/api/Client/Get',
                method: 'GET',
                params: request
            });
        };
        function _Get(id) {
            return $http({
                url: '/api/Client/GetById/' + id,
                method: 'GET'
            });
        };
        function _Put(request) {
            return $http({
                url: '/api/Client/Put',
                method: 'PUT',
                data: request
            });
        };
        function _Post(request) {
            return $http({
                url: '/api/Client/Post',
                method: 'POST',
                data: request
            });
        };

        function _Excel(request) {
            return $http({
                url: '/api/Client/excel',
                method: 'POST',
                data: request
            });
        }
    };
})();