(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('holidayService', Service);

    Service.$inject = ['$http', '$q'];
    function Service($http, $q) {
        var cities = [{ Name: 'São Paulo', Id: 1 }, { Name: 'Sorocaba', Id: 2 }];
        var listItensPrototype = [
            { Description: "Feriado 1", Date: moment().subtract(5, 'd') },
            { Description: "Feriado 2", Date: moment().add(1, 'd'), City: cities[1] }
        ];

        var service = {
            list: _List,
            get: _Get,
            put: _Put,
            post: _Post,
            delete: _Delete
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
        function _Get(id) {
            var response = {
                data: listItensPrototype.where(function (item) { return item.Id == id; }),
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _Put(request) {
            var city = cities.first(function (item) { return item.Id == request.City });
            var _resp = angular.copy(request);
            _resp.City = city;

            var response = {
                data: _resp,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _Post(request) {
            var city = cities.find(function (item) { return item.Id == request.City });
            var _resp = angular.copy(request);
            _resp.City = city;
            _resp.Id = listItensPrototype.length;
            //listItensPrototype.push(_resp)

            var response = {
                data: _resp,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _Delete(id) {
            var index = listItensPrototype.findIndex(function (item) { return item.Id == id; });
            listItensPrototype.splice(index, 1);
            var response = {
                data: {},
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
    };
})();