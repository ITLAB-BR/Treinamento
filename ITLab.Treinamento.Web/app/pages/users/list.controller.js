(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('userListController', controller);

    controller.$inject = ['$state', 'userService', 'commonsService', 'DatatablesOptions', 'DTColumnDefBuilder'];
    function controller($state, services, commonsService, DatatablesOptions, DTColumnDefBuilder) {
        var viewModel = this;

        viewModel.filters = {
            name: '',
            email: '',
            status: {
                listItems: getStatus(),
                selectedItem: getStatus()[0]
            }
        };

        viewModel.listItems = [];
        viewModel.functions = {
            searchFilter: searchFilter,
            resetFilter: clearFilters
        };

        (function initializePage() {
            clearFilters();
            datatablesConfig();
            setTimeout(searchFilter, 1000); // evita conflito das animações css
        })();

        function searchFilter() {
            var request = {
                name: viewModel.filters.name,
                email: viewModel.filters.email,
                active: viewModel.filters.status.selectedItem.value
            };

            beforeSendForm();
            services.getList(request).then(function (response) {
                afterSendForm();

                if (response.status != 200) {
                    commonsService.showMessage('alerts:error.list_user', 'alerts:error.error');
                    return;
                }
                viewModel.listItems = response.data;

                viewModel.listItems.forEach(function (item, index) {
                    if (item.AuthenticationTypeId == 1) {
                        item.AuthenticationTypeDescription = i18n.t('label.db');
                    } else if (item.AuthenticationTypeId == 2) {
                        item.AuthenticationTypeDescription = i18n.t('label.ad');
                    }
                });
            });
        }

        function datatablesConfig() {
            viewModel.dtOptions = angular.copy(DatatablesOptions)
                                    .withOption('order', [1, 'asc'])
            var i = 0;
            viewModel.dtColumns = [
                DTColumnDefBuilder.newColumnDef(i++).notSortable(),//action
                DTColumnDefBuilder.newColumnDef(i++),              //name
                DTColumnDefBuilder.newColumnDef(i++),              //email
                DTColumnDefBuilder.newColumnDef(i++),              //type
                DTColumnDefBuilder.newColumnDef(i++),              //active
            ];
        }

        function getStatus() {
            return [
                { Id: 0, value: null, text: i18n.t('label.all') },
                { Id: 1, value: true, text: i18n.t('label.active') },
                { Id: 2, value: false, text: i18n.t('label.inactive') }
            ];
        }

        function beforeSendForm() {
            viewModel.tableSpinnerShow = true;
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
        }

        function afterSendForm() {
            viewModel.tableSpinnerShow = false;
            commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
        }

        function clearFilters() {
            viewModel.filters.name = '';
            viewModel.filters.email = '';
            viewModel.filters.status.selectedItem = getStatus()[0];
        }
    }

})();