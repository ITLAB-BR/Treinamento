(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .controller('NotificationsController', Controller);

    Controller.$inject = ['notificationService']
    function Controller(itemService) {
        var viewModel = this;
        viewModel.listItems = [];

        viewModel.functions = {
            hasSelectedNotifications: _HasSelectedNotifications,
            refresh: _Refresh,
            markAsRead: _MarkAsRead,
            remove: _Remove
        };

        (function initializePage() {
            _Refresh();
        })();

        function _HasSelectedNotifications() {
            return viewModel.listItems.any(function (n) { return n.check; });
        };

        function _Refresh() {
            itemService.getNotification().then(function (dataResult) {
                viewModel.listItems = dataResult.data;
            });
        };

        function _MarkAsRead() {
            var selectedIdsToMarkAsRead = viewModel.listItems.where(function (n) { return n.check });
            if (!selectedIdsToMarkAsRead.any()) return;

            var request = selectedIdsToMarkAsRead.select(function (n) { return n.id });
            itemService.markAsRead(request).then(function () {
                selectedIdsToMarkAsRead.forEach(function (n) { n.check = false; });
                toastr.success(i18n.t('alerts:success.save_generic'), i18n.t('alerts:success.success'));
            });
        };

        function _Remove(not) {
            var selectedidsToRemove = viewModel.listItems.where(function (n) { return n.check });
            if (!selectedidsToRemove.any()) return;

            itemService.remove(selectedidsToRemove.select(function (n) { return n.id }))
                .then(function () {
                    _Refresh();
                    toastr.success(i18n.t('alerts:success.delete_generic'), i18n.t('alerts:success.success'));
                });
        };
    };
})();