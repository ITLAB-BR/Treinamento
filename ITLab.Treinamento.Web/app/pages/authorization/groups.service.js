(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .factory('groupsService', service);

    service.$inject = ['$http'];
    function service($http) {
        var service = {
            getList: getList,
            postItem: postItem,
            putItem: putItem,

            setPermissionToUsers: setPermissionToUsers,

            getRoles: getRoles,
            setRolesToGroup: setRolesToGroup
        };
        return service;

        function getList() {
            return $http({
                url: '/api/Group/List',
                method: 'GET'
            });
        }

        function postItem(request) {
            return $http({
                url: '/api/Group/CreateAsync',
                method: 'POST',
                data: request
            });
        }

        function putItem(request) {
            return $http({
                url: '/api/Group/UpdateAsync',
                method: 'PUT',
                data: request
            });
        }

        function getRoles() {
            return $http({
                url: '/api/Role/List',
                method: 'GET'
            });
        }

        function setPermissionToUsers(request) {
            return $http({
                url: '/api/Group/UpdateUsersAsync',
                method: 'PUT',
                data: request
            });
        }

        function setRolesToGroup(request) {
            return $http({
                url: '/api/Group/UpdatePermissionsAsync',
                method: 'PUT',
                data: request
            });
        }
    }

})();