(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('groupsService', service);

    service.$inject = ['$q'];
    function service($q) {
        var dataBasePrototype = {
            groups: [
                { Id: 1, Name: 'Administração do sistema', Roles: [-3, -2, -1, 0], Users: [1] },
                { Id: 2, Name: 'Análise', Roles: [-3, 1], Users: [1, 2] }
            ],
            roles: [
                { Id: -3, Name: 'Usuário alterar própria senha' },
                { Id: -2, Name: 'Gerenciar os parâmetros gerais do sistema' },
                { Id: -1, Name: 'Gerenciar perfis de acesso ao sistema' },
                { Id: 0, Name: 'Gerenciar usuários' },
                { Id: 1, Name: 'Gerenciar Paises' }
            ]
        };

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
            var deferred = $q.defer();

            var result = angular.copy(dataBasePrototype.groups);

            var response = {
                data: result,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }

        function postItem(request) {
            var nextId = dataBasePrototype.groups.length + 1

            var item = {
                Id: nextId,
                Name: request.Name,
                Roles: [],
                Users: []
            };

            dataBasePrototype.groups.push(item);

            var deferred = $q.defer();

            var response = {
                data: item,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }

        function putItem(request) {
            var item = dataBasePrototype.groups.where(function (item) { return item.Id == request.Id }).first();
            if (item) {
                item.Name = request.Name;
            }

            var deferred = $q.defer();

            var response = {
                data: request,
                status: 200
            };

            deferred.resolve(response);
            return deferred.promise;
        }

        function getRoles() {
            var deferred = $q.defer();

            var result = dataBasePrototype.roles;

            var response = {
                data: result,
                status: 200
            };

            deferred.resolve(response);
            return deferred.promise;
        }

        function setPermissionToUsers(request) {
            var group = dataBasePrototype.groups.where(function (item) { return item.Id == request.GroupId }).first();
            group.Users = request.UsersIds;

            var response = { status: 200 };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }

        function setRolesToGroup(request) {
            var group = dataBasePrototype.groups.where(function (item) { return item.Id == request.GroupId }).first();
            group.Roles = request.RolesIds;

            var response = { status: 200 };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }
    }

})();