

(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('HolidayController', Controller);

    Controller.$inject = ['$rootScope', 'holidayService', 'commonsService', 'DaterangepickerOptions'];
    function Controller($rootScope, itemService, commonsService, DaterangepickerOptions) {
        var viewModel = this;
        viewModel.holidayTypeEnum = { nacional: 0, municipal: 1 };

        var itemDefault = {
            action: 'new',
            index: undefined,
            Id: 0,
            Date: moment(),
            Description: '',
            HolidayType: viewModel.holidayTypeEnum.nacional
        };
        viewModel.itemEditing = null;
        viewModel.listItems = [];
        viewModel.dateConfig = DaterangepickerOptions.singleConfig;
        viewModel.cities = [{ Name: 'São Paulo', Id: 1 }, { Name: 'Sorocaba', Id: 2 }];

        viewModel.functions = {
            newItem: _NewItem,
            selectItem: _SelectItem,
            saveItem: _SaveItem,
            removeItem: _RemoveItem
        };

        (function initializePage() {
            _ListItems();
            viewModel.itemEditing = angular.copy(itemDefault);
        })();

        function _ListItems() {
            itemService.list().then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.data.Error);
                    return;
                }
                viewModel.listItems = response.data;
            });
        };

        function _NewItem() {
            viewModel.itemEditing = angular.copy(itemDefault);
            viewModel.itemEditing.action = 'new';
        };
        function _SelectItem(item) {
            viewModel.itemEditing = angular.copy(item);
            viewModel.itemEditing.index = viewModel.listItems.indexOf(item);
            viewModel.itemEditing.action = 'edit';
        };
        function _SaveItem() {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            var request = angular.copy(viewModel.itemEditing);

            if (viewModel.itemEditing.action == 'edit') {
                _ChangeInDatabase(request);
            } else if (viewModel.itemEditing.action == 'new') {
                _CreateInDatabase(request);
            }
        };

        function _ChangeInDatabase(request) {
            itemService.put(request).then(function (response) {
                viewModel.listItems[viewModel.itemEditing.index] = response.data;
                _FinalizeSave(response);
            });
            return false;
        };
        function _CreateInDatabase(request) {
            itemService.post(request).then(function (response) {
                viewModel.listItems.push(response.data);
                _FinalizeSave(response);
            });
            return false;
        };

        function _FinalizeSave(response) {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(false);

            if (response.status != 200) {
                commonsService.showMessage(response.data.message);
                return;
            }

            _NewItem();
            commonsService.showMessage('alerts:success.save_generic');
            $('.modal').modal('hide');
        };

        function _RemoveItem(id) {
            itemService.delete(id).then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.data.message);
                    return;
                }
                commonsService.showMessage('alerts:success.delete_generic');

                var index = viewModel.listItems.findIndex(function (item) { return item.Id == id; });
                viewModel.listItems.slice(index, 1);

                $('.modal').modal('hide');
            })
        };
    };
})();