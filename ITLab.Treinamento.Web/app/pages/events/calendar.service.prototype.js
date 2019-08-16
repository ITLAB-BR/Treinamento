(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('calendarService', Service);

    Service.$inject = ['$http', '$q'];
    function Service($http, $q) {
        var date = new Date();
        var d = date.getDate();
        var m = date.getMonth();
        var y = date.getFullYear();

        var listItensPrototype = [
            { Id: 1, Description: "teste", Start: moment(), End: moment().add(5, 'h'), Color: '#FF00FF' },
            { Id: 2, Description: "teste all day", Start: moment().add(1, 'd'), AllDay: true }
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
            var index = listItensPrototype.findIndex(function (item) { return item.Id == request.Id; });
            var itemResult = transform(request);
            listItensPrototype[index] = itemResult;

            var response = {
                data: itemResult,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _Post(request) {
            var itemResult = transform(request);
            itemResult.Id = listItensPrototype.lenth;
            var response = {
                data: itemResult,
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

        function transform(item) {
            item.Start = moment(item.Date + "T" + item.Start);
            item.End = moment(item.Date + "T" + item.End);
            return item;
        };
    };
})();