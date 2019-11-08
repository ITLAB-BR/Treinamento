angular
    .module('itlabtreinamento')
    .service('topNotificationService', ['notificationService', 'generalConfig', function (itemService, generalConfig) {
        var configDefaultRefreshNotificationTimeInSeconds = !!!generalConfig.defaultRefreshNotificationTimeInSeconds ? 10 : generalConfig.defaultRefreshNotificationTimeInSeconds;
        var currentRefreshNotificationTimeInSeconds = configDefaultRefreshNotificationTimeInSeconds;
        var notificationIsVisible = true;
        var amountNewNotifications = 0;
        var intervalIdentification = 0;
        var listItems = [];

        var service = {
            toggleNotifications: toggleNotifications,
            getNotifications: getNotifications,
            refreshNotifications: refreshNotifications,
            stopRefreshNotification: stopRefreshNotification,

            getAmountNewNotifications: function () { return amountNewNotifications; },
            getListItems: function () { return listItems; }
        };
        return service;

        function toggleNotifications() {
            notificationIsVisible = !notificationIsVisible;
            if (amountNewNotifications > 0) {
                itemService.confirmReadNotifications();
                amountNewNotifications = 0;
            }

            currentRefreshNotificationTimeInSeconds = 30;
            refreshNotifications();
        }

        var callbackForGetNotifications;

        function getNotifications(callback) {
            var totalRecords = 5;
            itemService.getNotification(totalRecords).then(function (response) {
                if (response.status !== 200) { stopRefreshNotification(); return; }

                amountNewNotifications = response.data.count(function (d) { return !d.readIn });
                listItems = response.data;

                if (typeof callback === 'function')
                    callbackForGetNotifications = callback;
                if (typeof callbackForGetNotifications === 'function')
                    callbackForGetNotifications();
            });

            if (currentRefreshNotificationTimeInSeconds !== generalConfig.defaultRefreshNotificationTimeInSeconds) {
                currentRefreshNotificationTimeInSeconds = generalConfig.defaultRefreshNotificationTimeInSeconds;
                refreshNotifications();
            }
        }

        function refreshNotifications() {
            stopRefreshNotification();
            intervalIdentification = setInterval(getNotifications, currentRefreshNotificationTimeInSeconds * 1000);
        }

        function stopRefreshNotification() {
            clearInterval(intervalIdentification);
        }
    }])
    .directive('topnavbar', function () {
        return {
            restrict: 'E',
            templateUrl: '/app/directives/elements/topnavbar.html',
            controller: ['$scope', 'principal', 'multilanguage', 'generalConfig', 'topNotificationService', 'signoutService', function ($scope, principal, multilanguage, generalConfig, topNotification, signoutService) {
                $scope.viewModel = {};
                var viewModel = $scope.viewModel;

                viewModel.language = multilanguage.currentLanguage;
                viewModel.availableLanguages = generalConfig.language.availableLanguages;
                viewModel.amountNewNotifications = null;
                viewModel.listItems = null;

                viewModel.functions = {
                    changeLanguage: changeLanguage,
                    signout: signoutService,
                    toggleNotifications: toggleNotifications
                };

                (function initializePage() {
                    topNotification.getNotifications(updateDataNotification);
                    topNotification.refreshNotifications();
                })();

                function toggleNotifications() {
                    topNotification.toggleNotifications();
                    viewModel.amountNewNotifications = topNotification.getAmountNewNotifications();
                }

                function updateDataNotification() {
                    viewModel.listItems = topNotification.getListItems();
                    viewModel.amountNewNotifications = topNotification.getAmountNewNotifications();
                }

                function changeLanguage(newLanguage) {
                    viewModel.language = newLanguage;
                    multilanguage.setLanguage(newLanguage);
                }
            }]
        };
    });