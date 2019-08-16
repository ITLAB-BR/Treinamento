(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('countryService', service);

    service.$inject = ['$q'];
    function service($q) {

        var dataBasePrototype = {
            country: [{ Id: 1, Name: 'Brasil', Active: true },
                      { Id: 2, Name: 'Argentina', Active: true }]
        };

        var service = {
            list: list,
            get: get,
            put: put,
            post: post
        };
        return service;

        function list() {
            var deferred = $q.defer();
            var response = {
                data: dataBasePrototype.country,
                status: 200
            };

            deferred.resolve(response);
            return deferred.promise;
        }

        function get(id) {
            var response = {
                data: dataBasePrototype.country.where(function (item) { return item.Id == id; }),
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }

        function put(request) {
            var response = {
                data: request,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }

        function post(request) {
            request.Id = dataBasePrototype.country.lenth;
            var response = {
                data: request,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }
    }

})();