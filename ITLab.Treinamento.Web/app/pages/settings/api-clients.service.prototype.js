(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('apiClientService', Service);

    Service.$inject = ['$http', '$q'];
    function Service($http, $q) {
        var listItensPrototype = [
            { Active: true, Id: 'WebAngularAppAuth', Name: 'AngularJs Front-End Application', Type: 1, AllowedOrigin: "http://localhost:8080", RefreshTokenLifeTimeInMinutes: 7200, Secret: null },
            { Active: true, Id: 'ConsoleAppAuth', Name: 'Application Console Test', Type: 2, AllowedOrigin: "*", RefreshTokenLifeTimeInMinutes: 7200, Secret: null },
        ];

        var service = {
            list: _List,
            post: _Post,
            put: _Put,

            putSecret: _PutSecret
        };
        return service;

        function _List() {
            var response = {
                data: listItensPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _Put(request) {
            var response = {
                data: request,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _Post(request) {
            var response = {
                data: request,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };

        function _PutSecret(request) {
            var response = {
                data: request,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
    };
})();