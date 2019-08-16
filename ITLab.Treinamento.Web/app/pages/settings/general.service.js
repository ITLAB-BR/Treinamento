(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('generalSettingsService', service);

    service.$inject = ['$http'];
    function service($http) {
        var activeDirectoryTypes = {
            local: 1,
            server: 2
        };

        var service = {
            activeDirectoryTypes: activeDirectoryTypes,

            get: get,
            putItem: putItem
        };
        return service;

        function get(refresh, setting) {
            var refresh = !!refresh;

            return $http({
                url: '/api/Settings/Get?refresh=' + refresh,
                method: 'GET'
            }).then(function (response) {
                if (!setting) return response;

                angular.forEach(response.data, function (value, key) {
                    if (setting == key)
                        return value;
                });
            });
        }

        function putItem(request) {
            return $http({
                url: '/api/Settings/Set',
                method: 'PUT',
                data: request
            });
        }
    }

})();