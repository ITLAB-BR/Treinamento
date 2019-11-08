(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .controller('OrderController', OrderController)
        .run(run);

    OrderController.$inject = ['$state', 'orderServicePrototype', 'commonsService'];
    function OrderController($state, itemService, commonsService) {
        if (!!$state.params.id) {
            var id = angular.fromJson($state.params.id);
        }

        var viewModel = this;

        var itemDefault = {
            action: 'new',
            Id: 0,
            Date: moment(),
            ClientId: null,
            Client: null,
            OrderItens: [],
            Total: 0
        };
        var itemDetailDefault = {
            ProductId: null,
            Product: null,
            Amount: 1
        };
        viewModel.itemEditing = angular.copy(itemDefault);
        viewModel.itemDetailEditing = angular.copy(itemDetailDefault);
        viewModel.listItems = [];

        viewModel.clients = [];
        viewModel.products = [];

        viewModel.functions = {
            saveItem: _SaveItem,
            addItemOnDetailList: _AddItemOnDetailList,
            askConfirmRemoveItemOnDetailList: _AskConfirmRemoveItemOnDetailList,
            removeItemOnDetailList: null,
            calcTotal: _CalcTotal,
            getClients: _GetClients,
            getProducts: _GetProducts
        };

        (function initializePage() {
            if (id)
                _GetItemEdit();
        })();

        function _SaveItem() {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            var request = angular.copy(viewModel.itemEditing);

            if (viewModel.itemEditing.action == 'edit') {
                _ChangeInDatabase(request);
            } else if (viewModel.itemEditing.action == 'new') {
                _CreateInDatabase(request);
            }
        };

        function _AddItemOnDetailList() {
            viewModel.itemEditing.OrderItens.push(angular.copy(viewModel.itemDetailEditing));
            viewModel.itemDetailEditing = angular.copy(itemDetailDefault);

            $('.modal').modal('hide');
        };
        function _AskConfirmRemoveItemOnDetailList(index) {
            return function _Remove() {
                viewModel.itemEditing.OrderItens.splice(index, 1);

                $('.modal').modal('hide');
            };
        };

        function _CalcTotal() {
            if (!viewModel.itemEditing.OrderItens) return;

            var itens = viewModel.itemEditing.OrderItens;
            var total = 0;
            for (var i = 0; i < itens.length; i++) {
                total += itens[i].Product.Price * itens[i].Amount;
            }
            return viewModel.itemEditing.Total = total;
        };

        function _GetItemEdit() {
            itemService.get(id).then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.Error);
                    return;
                }

                viewModel.itemEditing = response.data[0];
                viewModel.itemEditing.action = 'edit';
            });
        };
        function _GetClients(query) {
            commonsService.getClients(query).then(function (response) {
                if (response.status !== 200) {
                    commonsService.showMessage(response.message);
                    return;
                }
                viewModel.clients = response.data;
            });
        };
        function _GetProducts(query) {
            itemService.getProducts(query).then(function (response) {
                if (response.status !== 200) {
                    commonsService.showMessage(response.message);
                    return;
                }

                viewModel.products = response.data;
            });
        };

        function _ChangeInDatabase(request) {
            itemService.put(request).then(function (response) {
                viewModel.itemEditing = response.data;
                _FinalizeSave(response);
            });
            return false;
        };
        function _CreateInDatabase(request) {
            itemService.post(request).then(function (response) {
                viewModel.itemEditing = response.data;
                viewModel.itemEditing.action = 'edit';
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

            commonsService.showMessage('alerts:success.save_generic');
        };
    };

    function run(editableOptions, editableThemes) {
        // mover isso para config
        editableOptions.theme = 'bs3'; // bootstrap3
    };
})();