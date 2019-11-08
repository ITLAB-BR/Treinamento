(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('accountEditService', Service);

    Service.$inject = ['$http', '$q'];
    function Service($http, $q) {
        var listItensPrototype = {
            Login: 'admin',
            Email: 'admin@system.com',
            Name: 'Administrator',
            Photo: null
        }
        var service = {
            get: _Get,
            set: _Set
        };
        return service;

        function _Get() {
            var response = {
                data: listItensPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _Set(request) {
            var response = {
                data: listItensPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
    };
})();