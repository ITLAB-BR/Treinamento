(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('userService', service);

    service.$inject = ['$http', 'countryService'];
    function service($http, countryService) {
        var authenticationTypes = {
            dataBase: 1,
            activeDirectory: 2
        };

        var service = {
            getList: getList,
            getItem: getItem,
            postItem: postItem,
            putItem: putItem,

            getListGroups: getListGroups,
            getListCountries: getListCountries,

            authenticationTypes: authenticationTypes
        };
        return service;

        function getList(request) {
            return $http({
                url: '/api/User/GetList',
                method: 'GET',
                params: request
            });
        }

        function getItem(id) {
            return $http({
                url: '/api/User/GetItem/' + id,
                method: 'GET'
            });
        }

        function postItem(request) {
            return $http({
                url: '/api/User/CreateAsync',
                method: 'POST',
                data: request
            });
        }

        function putItem(request) {
            return $http({
                url: '/api/User/UpdateAsync',
                method: 'put',
                data: request
            });
        }

        function getListGroups() {
            return $http({
                url: '/api/Group/List',
                method: 'GET'
            });
        }

        function getListCountries() {
            return countryService.list().then();
        }
    }

})();