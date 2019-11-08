(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('clientService', Service);

    Service.$inject = ['$http', '$q'];
    function Service($http, $q) {
        var listItensPrototype = [
          { Id: 1, Name: 'Cliente A', Active: true, Email: 'user.a@user.com', Telephone: 9912345678, CPF: 121231231, CNPJ: undefined, BirthDate: moment('1988-02-04') },
          { Id: 2, Name: 'Cliente B', Active: true, Email: 'user.b@user.com', Telephone: 9912345678, CPF: 121231239, CNPJ: undefined, BirthDate: moment('2001-12-04') },
          { Id: 3, Name: 'Cliente C', Active: false, Email: 'user.c@user.com', Telephone: 9912345678, CPF: undefined, CNPJ: 12345678000112, BirthDate: null },
          { Id: 4, Name: 'Cliente D', Active: true, Email: 'user.d@user.com', Telephone: 9912345678, CPF: undefined, CNPJ: 12345678000167, BirthDate: null },
        ];

        var service = {
            list: _List,
            get: _Get,
            put: _Put,
            post: _Post,
            excel: _Excel
        };
        return service;

        function _List() {
            var response = {
                data: {
                    table: listItensPrototype,
                    recordsTotal: listItensPrototype.length
                },
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _Get(id) {
            var response = {
                data: listItensPrototype.first(function (item) { return item.Id == id; }),
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
            request.Id = listItensPrototype.length;
            var response = {
                data: request,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };

        function _Excel(request) {
            var response = {
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }
    };
})();