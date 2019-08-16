(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('holidayService', Service);

    Service.$inject = ['$http'];
    function Service($http) {
        var service = {
            list: _List,
            get: _Get,
            put: _Put,
            post: _Post
        };
        return service;

        function _List() {
            return $http({
                url: '/api/ApiContollerName/Get',
                method: 'GET',
                params: { onlyByUserLogged: false, onlyActive: false }
            });
        };
        function _Get(id) {
            return $http({
                url: '/api/ApiContollerName/Get/' + id,
                params: { onlyByUserLogged: true },
                method: 'GET'
            });
        };
        function _Put(request) {
            return $http({
                url: '/api/ApiContollerName/Put',
                method: 'PUT',
                data: request
            });
        };
        function _Post(request) {
            return $http({
                url: '/api/ApiContollerName/Post',
                method: 'POST',
                data: request
            });
        };
    };
})();