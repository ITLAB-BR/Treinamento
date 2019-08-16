(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('countryController', controller);

    controller.$inject = ['countryService', 'commonsService'];
    function controller(itemService, commonsService) {
        var viewModel = this;

        var itemDefault = {
            action: 'new',
            index: undefined,
            Id: 0,
            Name: '',
            Active: true
        };

        viewModel.listItems = [];
        viewModel.itemEditing = null;
        viewModel.functions = {
            newItem: newItem,
            selectItem: selectItem,
            saveItem: saveItem
        };

        (function initializePage() {
            listItems();
            viewModel.itemEditing = angular.copy(itemDefault);
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
            viewModel.itemEditing.action = 'new';
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
            } else if (viewModel.itemEditing.action == 'new') {
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
    }

})();