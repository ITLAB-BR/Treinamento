(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('countryService', service);

    service.$inject = ['$http'];
    function service($http) {
        var service = {
            list: list,
            get: get,
            put: put,
            post: post
        };
        return service;

        function list() {
            return $http({
                url: '/api/Country/Get',
                method: 'GET',
                params: { onlyByUserLogged: false, onlyActive: false }
            });
        }

        function get(id) {
            return $http({
                url: '/api/Country/Get/' + id,
                params: { onlyByUserLogged: true },
                method: 'GET'
            });
        }

        function put(request) {
            return $http({
                url: '/api/Country/Put',
                method: 'PUT',
                data: request
            });
        }

        function post(request) {
            return $http({
                url: '/api/Country/Post',
                method: 'POST',
                data: request
            });
        }
    }

})();