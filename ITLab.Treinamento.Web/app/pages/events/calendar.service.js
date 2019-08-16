(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('calendarService', Service);

    Service.$inject = ['$http'];
    function Service($http) {
        var service = {
            list: _List,
            get: _Get,
            put: _Put,
            post: _Post,
            delete: _Delete
        };
        return service;

        function _List(request) {
            return $http({
                url: '/api/Event/Get',
                method: 'GET',
                params: request
            });
        };
        function _Get(id) {
            return $http({
                url: '/api/Event/Get/' + id,
                method: 'GET',
                params: {}
            });
        };
        function _Put(request) {
            return $http({
                url: '/api/Event/Put',
                method: 'PUT',
                data: request
            });
        };
        function _Post(request) {
            return $http({
                url: '/api/Event/Post',
                method: 'POST',
                data: request
            });
        };
        function _Delete(id) {
            return $http({
                url: '/api/Event/Delete/' + id,
                method: 'DELETE'
            });
        }
    };
})();