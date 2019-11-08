(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('notificationService', service);

    service.$inject = ['$http'];
    function service($http) {
        var service = {
            getNotification: getNotifications,
            confirmReadNotifications: confirmReadNotifications,
            markAsRead: markAsRead,
            remove: remove
        };
        return service;

        function getNotifications(totalRecords) {
            return $http({
                url: '/api/notification/get',
                method: 'GET',
                params: { totalRecords: totalRecords }
            });
        }

        function confirmReadNotifications() {
            return $http({
                url: '/api/notification/confirmRead',
                method: 'POST'
            });
        }

        function markAsRead(notificationIds) {
            return $http({
                url: '/api/notification/markAsRead',
                method: 'PUT',
                params: { notifications: notificationIds }
            });
        }

        function remove(notificationIds) {
            return $http({
                url: '/api/notification/remove',
                method: 'DELETE',
                params: { notifications: notificationIds }
            });
        }
    }

})();