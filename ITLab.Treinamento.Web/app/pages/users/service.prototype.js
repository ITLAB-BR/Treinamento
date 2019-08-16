(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('userService', service);

    service.$inject = ['$q', 'countryService'];
    function service($q, countryService) {
        var authenticationTypes = {
            dataBase: 1,
            activeDirectory: 2
        };

        var dataBasePrototype = {
            users: [
            {
                Id: 1,
                Active: true,
                AuthenticationTypeId: authenticationTypes.activeDirectory,
                Name: 'Administrator (Prototype)',
                Login: 'admin',
                Email: 'admin@itlab.com.br',
                AccessAllDataVisibility: true,
                IsPasswordExpired: false,
                LastPasswordChangedDate: null,
                DateThatMustChangePassword: null,
                DaysLeftToChangePassword: null,
                UserBlockedForManyAccess: null,
                LockoutEndDateUtc: null,
                Groups: [1],
                Countries: [],

                Senha: '123456'
            },
            {
                Id: 2,
                Active: true,
                AuthenticationTypeId: authenticationTypes.dataBase,
                Name: 'User 1 (Prototype)',
                Login: 'user1',
                Email: 'user@itlab.com.br',
                AccessAllDataVisibility: false,
                IsPasswordExpired: false,
                LastPasswordChangedDate: moment().subtract(5, 'd'),
                DateThatMustChangePassword: moment().add(5, 'd'),
                DaysLeftToChangePassword: 5,
                UserBlockedForManyAccess: false,
                LockoutEndDateUtc: null,
                Groups: [],
                Countries: [1],

                Senha: '0000'
            }],
            groups: [{
                Id: 1,
                Name: 'Administração do sistema',
                Roles: [-4, -3, -2, -1, 0, 1],
                Users: [1]
            }]
        };

        var service = {
            getList: getList,
            getItem: getItem,
            postItem: postItem,
            putItem: putItem,

            getListGroups: getListGroups,
            getListCountries: getListCountries,

            authenticationTypes: authenticationTypes,

            prototypeMode: true
        };
        return service;

        function getList(request) {
            var deferred = $q.defer();

            var result = dataBasePrototype.users;

            if (request != null) {
                result = result.where(function (item) {
                    return (request.active == undefined || item.Active == request.active)
                            && item.Name.toLowerCase().indexOf(request.name.toLowerCase()) != -1
                            && item.Email.toLowerCase().indexOf(request.email.toLowerCase()) != -1;
                });
            }

            var response = {
                data: result,
                status: 200
            };

            deferred.resolve(response);
            return deferred.promise;
        }

        function getItem(id) {
            var deferred = $q.defer();
            var response = {
                data: dataBasePrototype.users.where(function (item) { return item.Id == id; }).first(),
                status: 200
            };

            deferred.resolve(response);
            return deferred.promise;
        }

        function postItem(request) {
            var nextId = dataBasePrototype.users.length + 1;

            var item = {
                Id: nextId,
                Active: true,
                AuthenticationTypeId: request.AuthenticationTypeId,
                Name: request.Name,
                Login: request.Login,
                Email: request.Email,
                AccessAllDataVisibility: false,
                IsPasswordExpired: false,
                UserBlockedForManyAccess: false,
                LockoutEndDateUtc: null,
                Groups: [],
                Countries: [],

                Senha: request.Password1
            };

            if (item.AuthenticationTypeId == authenticationTypes.dataBase) {
                item.LastPasswordChangedDate = moment().subtract(0, 'd');
                item.DateThatMustChangePassword = moment().add(10, 'd');
                item.DaysLeftToChangePassword = 10;
            }

            dataBasePrototype.users.push(item);

            var deferred = $q.defer();

            var response = {
                data: item.Id,
                status: 200
            };

            deferred.resolve(response);
            return deferred.promise;
        }

        function putItem(request) {
            var user = dataBasePrototype.users.where(function (item) { return item.Id == request.Id }).first();
            if (user) {
                user.Active = request.Active;
                user.Name = request.Name;
                user.AccessAllDataVisibility = request.AccessAllDataVisibility;
                user.Groups = request.Groups;

                if (user.AccessAllDataVisibility) {
                    user.Countries = [];
                } else {
                    user.Countries = request.Countries;
                }
            }

            var deferred = $q.defer();

            var response = {
                status: 200
            };

            deferred.resolve(response);
            return deferred.promise;
        }

        function getListGroups() {
            var deferred = $q.defer();

            var result = dataBasePrototype.groups;

            var response = {
                data: result,
                status: 200
            };

            deferred.resolve(response);
            return deferred.promise;
        };

        function getListCountries() {
            return countryService.list().then();
        }
    }

})();