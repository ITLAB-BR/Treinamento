(function () {
    'use strict';

    angular.module('itlabtreinamento')
           .controller('ApiClientController', controller)
           .filter('typeApiClient', typeApiClientFilter);

    controller.$inject = ['apiClientService', 'commonsService'];
    function controller(itemService, commonsService) {
        var viewModel = this;

        var itemDefault = {
            action: 'new',
            Active: true,
            Id: '',
            Name: '',
            Type: 0,
            AllowedOrigin: "*",
            RefreshTokenLifeTimeInMinutes: 7200,
            Secret: ''
        };

        viewModel.listItems = [];
        viewModel.itemEditing = null;
        viewModel.functions = {
            newItem: newItem,
            selectItem: selectItem,
            saveItem: saveItem,
            saveSecretKey: saveSecretKey
        };

        (function initializePage() {
            listItems();
        })();

        function listItems() {
            itemService.list().then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.data.Error);
                    return;
                }
                viewModel.listItems = response.data;
            });
        }

        function newItem() {
            viewModel.itemEditing = angular.copy(itemDefault);
        }

        function selectItem(item) {
            viewModel.itemEditing = angular.copy(item);
            viewModel.itemEditing.index = viewModel.listItems.indexOf(item);
            viewModel.itemEditing.action = 'edit';
        }

        function saveItem() {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            var request = angular.copy(viewModel.itemEditing);

            if (viewModel.itemEditing.action == 'edit') {
                changeInDatabase(request);
            } else {
                createInDatabase(request);
            }
        }

        function changeInDatabase(request) {
            itemService.put(request).then(function (response) {
                viewModel.listItems[viewModel.itemEditing.index] = response.data;
                finalizeSave(response);
            });
            return false;
        }

        function createInDatabase(request) {
            itemService.post(request).then(function (response) {
                viewModel.listItems.push(response.data);
                finalizeSave(response);
            });
            return false;
        }

        function finalizeSave(response) {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(false);

            if (response.status != 200) {
                commonsService.showMessage(response.data.message);
                return;
            }

            commonsService.showMessage('alerts:success.save_generic');
            $('.modal').modal('hide');
        }

        function saveSecretKey() {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            var request = {
                Id: viewModel.itemEditing.Id,
                Secret: viewModel.itemEditing.Secret
            };

            itemService.putSecret(request).then(function (response) {
                commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                viewModel.key = {};

                if (response.status != 200) {
                    commonsService.showMessage('alerts:error.save_generic');
                    return;
                }

                commonsService.showMessage('alerts:success.save_generic');
                $('.modal').modal('hide');
            });
        }
    }

    function typeApiClientFilter() {
        var typeDescription = { 1: 'page.api_clients.type_javascript', 2: 'page.api_clients.type_native' };
        return function (typeId) {
            return i18n.t(typeDescription[typeId]);
        };
    }

})();
