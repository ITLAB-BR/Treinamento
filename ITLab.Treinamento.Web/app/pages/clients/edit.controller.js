teste = '';
(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('ClientController', Service)
                           .constant('clientTypeEnum', { person: 1, company: 2 });

    Service.$inject = ['$scope', '$state', 'clientService', 'commonsService', 'clientTypeEnum', 'DaterangepickerOptions'];
    function Service($scope, $state, itemService, commonsService, clientTypeEnum, DaterangepickerOptions) {
        if (!!$state.params.id) {
            var id = angular.fromJson($state.params.id);
        }

        var viewModel = this;

        var itemDefault = {
            action: 'new',
            Id: undefined,
            Name: '',
            Active: true,
            Type: clientTypeEnum.person,
            Email: '',
            Telephone: undefined,
            CPF: undefined,
            CNPJ: undefined,
            BirthDate: null
        };
        viewModel.listItems = [];
        viewModel.itemEditing = null;
        viewModel.functions = {
            saveItem: _SaveItem
        };
        viewModel.dateBirthOptions = DaterangepickerOptions.singleConfig;

        viewModel.itemTypeEnum = clientTypeEnum;

        (function initializePage() {
            if (id) {
                _GetItemEdit();
            } else {
                _NewItem();
            }
        })();

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
            request.BirthDate = request.BirthDate && moment(viewModel.itemEditing.BirthDate).format();

            if (viewModel.itemEditing.action == 'edit') {
                _ChangeInDatabase(request);
            } else if (viewModel.itemEditing.action == 'new') {
                _CreateInDatabase(request);
            }
        };

        function _ChangeInDatabase(request) {
            itemService.put(request).then(_FinalizeSave);
            return false;
        };
        function _CreateInDatabase(request) {
            itemService.post(request).then(_FinalizeSave);
            return false;
        };

        function _FinalizeSave(response) {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(false);

            if (response.status != 200) {
                commonsService.showMessage(response.data.message);
                return;
            }

            commonsService.showMessage('alerts:success.save_generic');
            $('.modal').modal('hide');
        };

        function _GetItemEdit() {
            itemService.get(id).then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.Error);
                    return;
                }

                viewModel.itemEditing = response.data;
                viewModel.itemEditing.action = 'edit';
                if (viewModel.itemEditing.CNPJ)
                    viewModel.itemEditing.Type = clientTypeEnum.company;
                else
                    viewModel.itemEditing.Type = clientTypeEnum.person;

                viewModel.itemEditing.BirthDate = (viewModel.itemEditing.BirthDate) ? moment(viewModel.itemEditing.BirthDate) : null;
            });
        };
        
    };
})();