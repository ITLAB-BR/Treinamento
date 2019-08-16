(function () {
    angular.module('itlabtreinamento')
            .controller('OrdersController', Controller);
    Controller.$inject = ['orderServicePrototype', 'commonsService', 'searchFactory', 'formFactory', 'DTColumnDefBuilder', 'DatatablesOptions'];
    function Controller(itemService, commonsService, searchFactory, formFactory, DTColumnDefBuilder, DatatablesOptions) {
        var viewModel = this;

        var filterDefault = {
            Id: null,
            Client: null
        };
        viewModel.filter = null;
        viewModel.listItems = [];
        viewModel.functions = {
            searchFilter: _SearchFilter,
            resetFilter: _ResetFilter,

            getClients: _GetClients
        };

        (function initializePage() {
            _ResetFilter();
            setTimeout(_SearchFilter, 1000); // evita conflito das animações css
            _DatatablesConfig();
        })();


        function _SearchFilter() {
            var request = angular.copy(viewModel.filter) || {};

            _BeforeSendForm();
            itemService.list(request).then(function (response) {
                _AfterSendForm();

                if (response.status != 200) {
                    commonsService.showMessage(response.Error);
                    return;
                }
                viewModel.listItems = response.data;
            });
        };
        function _ResetFilter() {
            viewModel.filter = angular.copy(filterDefault);
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

        function _BeforeSendForm() {
            viewModel.tableSpinnerShow = true;
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
        };
        function _AfterSendForm() {
            viewModel.tableSpinnerShow = false;
            commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
        };

        function _DatatablesConfig() {
            viewModel.dtOptions = angular.copy(DatatablesOptions)
                                    .withOption('order', [4, 'desc']);

            var i = 0;
            viewModel.dtColumns = [
                DTColumnDefBuilder.newColumnDef(i++).notSortable(),//action
                DTColumnDefBuilder.newColumnDef(i++),              //number
                DTColumnDefBuilder.newColumnDef(i++),              //client
                DTColumnDefBuilder.newColumnDef(i++),              //value
                DTColumnDefBuilder.newColumnDef(i++),              //date
            ];
        };
    };
})();