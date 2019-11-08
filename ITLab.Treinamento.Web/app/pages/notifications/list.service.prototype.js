(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('notificationService', service);

    service.$inject = ['$q'];
    function service($q) {
        var dataBasePrototype = {
            notifications: [
                { id: 2, readIn: false, message: 'Exemplo de mensagem para protótipo', date: new Date() },
                { id: 1, readIn: moment().subtract(20, 'm').toDate(), message: 'Seja bem-vindo ao sistema!!!', date: moment().subtract(30, 'm').toDate() }
            ]
        };

        var service = {
            getNotification: getNotifications,
            confirmReadNotifications: confirmReadNotifications,
            markAsRead: markAsRead,
            remove: remove
        };
        return service;

        function getNotifications(totalRecords) {
            var response = {
                data: dataBasePrototype.notifications,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }

        function confirmReadNotifications() {
            var response = {
                data: dataBasePrototype.notifications,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }

        function markAsRead(notificationIds) {
            var itens = dataBasePrototype.notifications.where(function (item) { return notificationIds.any(function (id) { return item.id == id }); });
            for (var i = 0; i < itens.length; i++) {
                itens[i].readIn = new Date();
            }
            var response = { status: 200 };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }

        function remove(notificationIds) {
            dataBasePrototype.notifications = dataBasePrototype.notifications.where(function (item) { return notificationIds.any(function (id) { return item.id != id }); });
            var response = { status: 200 };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }
    }

})();