(function () {
    'use strict';


    angular.module('itlabtreinamento')
        .controller('ClientsController', ClientsController)
        .constant('clientTypeEnum', { person: 1, company: 2 });

    ClientsController.$inject = ['$scope', '$compile', 'clientService', 'commonsService',
        'searchFactory', 'formFactory', 'clientTypeEnum', 'DatatablesOptions', 'DTColumnBuilder', 'DTOptionsBuilder'];
    function ClientsController($scope, $compile, itemService, commonsService,
        searchFactory, formFactory, clientTypeEnum, DatatablesOptions, DTColumnBuilder, DTOptionsBuilder) {

        var viewModel = this;

        var filterDefault = {
            name: '',
            email: '',
            type: null,
            telephone: '',
            CNPJ: undefined,
            CPF: undefined,
            active: null
        };
        viewModel.filter = null;
        viewModel.dtInstance = {};
        viewModel.functions = {
            searchFilter: _RefreshDatatable,
            resetFilter: _ResetFilter
        };

        (function initializePage() {
            viewModel.statuses = searchFactory.getStatuses();
            viewModel.types = _GetTypes();

            _ResetFilter();
            _DatatablesConfig();
        })();

        function _RefreshDatatable() {
            viewModel.dtInstance.dataTable.fnFilter();
        };

        function getFilter() {
            var request = angular.copy(viewModel.filter) || {};
            request.CPF = formFactory.numberfy(request.CPF);
            request.CNPJ = formFactory.numberfy(request.CNPJ);
            request.telephone = formFactory.numberfy(request.telephone);

            return request;
        }

        function _SearchFilter(searchOptions) {
            var request = getFilter();
            angular.extend(request, searchOptions)

            _BeforeSendForm();
            return itemService.list(request).then(function (response) {
                _AfterSendForm();

                if (response.status != 200) {
                    commonsService.showMessage(response.Error);
                    return;
                }
                return response;
            });
        };

        function _DatatablesConfig() {
            viewModel.dtColumns = [
                DTColumnBuilder.newColumn(null).renderWith(insertButtons).withClass('text-center').notSortable(),
                DTColumnBuilder.newColumn('Name'),
                DTColumnBuilder.newColumn('CNPJ').renderWith(formatCnpj),
                DTColumnBuilder.newColumn('Email'),
                DTColumnBuilder.newColumn('Telephone').renderWith(formatPhone),
                DTColumnBuilder.newColumn('Active').renderWith(formatStatus).withClass('text-center'),
            ];

            viewModel.dtOptions = DatatablesOptions
                .appendButtons([{
                    extend: 'excel',
                    text: i18n.t('label.excel_all_pages'), //'Excel (all pages)'
                    action: function () {
                        // obtem a ordenação corrente da grid
                        var index = viewModel.dtInstance.DataTable.order()[0][0] || viewModel.dtInstance.DataTable.order()[0];
                        var order = viewModel.dtInstance.DataTable.order()[0][1] || 'asc';
                        var column = viewModel.dtColumns[index].mData;
    
                        // obtem os filtros que estão aplicados
                        var filter = angular.extend(getFilter(), { orderByColumn: column, orderByAsc: order === 'asc' });
    
                        // chama o endpoint específico para exportação para excel.
                        itemService.excel(filter).then(() => commonsService.showMessage('alerts:success.request_generate_file'));
                    }
                }])
                .withOption('searching', false)
                .withOption('order', [1, 'asc'])// inicia ordenado por Name
                .withOption('scrollX', true)
                .withFnServerData(serverData)
                .withOption('serverSide', true)
                .withOption('processing', true)
                .withOption('fnRowCallback', compileRow);

            viewModel.data = {};
            function compileRow(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
                $compile(nRow)($scope);
            };
            function formatCnpj(data, type, full, meta) {
                viewModel.data[meta.row] = viewModel.data[meta.row] || {};
                viewModel.data[meta.row].document = full.CNPJ || full.CPF;

                return full.CNPJ ?
                    '<span mask="99.999.999/9999-99" ng-bind="viewModel.data[' + meta.row + '].document" ng-model="viewModel.data[' + meta.row + '].document"></span>' :
                    '<span mask="99.999.999-9" ng-bind="viewModel.data[' + meta.row + '].document" ng-model="viewModel.data[' + meta.row + '].document"></span>'
            };
            function formatPhone(data, type, full, meta) {
                viewModel.data[meta.row] = viewModel.data[meta.row] || {};
                viewModel.data[meta.row].telefone = data;

                return '<span mask="(99) 9?9999-9999" ng-bind="viewModel.data[' + meta.row + '].telefone" ng-model="viewModel.data[' + meta.row + '].telefone"></span>';
            };
            function formatStatus(data, type, full, meta) {
                return '<status-label status="' + data + '"></status-label>';
            };
            function insertButtons(data, type, full, meta) {
                return '<a class="btn btn-xs btn-blue"\
                           ng-i18n-title="label.edit"\
                           ui-sref="clients-edit({ id:' + full.Id + ' })">\
                            <i class="fa fa-pencil fa-small fa-width-fixed"></i>\
                        </a>';
            };

            function serverData(sSource, aoData, fnCallback, oSettings) {
                var draw = aoData.find(function (i) { return i.name == 'draw' }).value,
                    colunms = aoData.find(function (i) { return i.name == 'columns' }).value,
                    order = aoData.find(function (i) { return i.name == 'order' }).value,
                    start = aoData.find(function (i) { return i.name == 'start' }).value,
                    length = aoData.find(function (i) { return i.name == 'length' }).value;

                var searchOptions = {
                    start: start,
                    length: length,
                    orderByColumn: colunms[order.first().column].data,
                    orderByAsc: order.first().dir == 'asc'
                };
                _SearchFilter(searchOptions).then(function (response) {
                    if (!response) return;
                    fnCallback({
                        draw: draw,
                        recordsTotal: response.data.recordsTotal,
                        recordsFiltered: response.data.recordsTotal,
                        data: response.data.table
                    });
                });
            };
        };

        function _BeforeSendForm() {
            viewModel.tableSpinnerShow = true;
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
        };
        function _AfterSendForm() {
            viewModel.tableSpinnerShow = false;
            commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
        };

        function _ResetFilter() {
            viewModel.filter = angular.copy(filterDefault);
        };
        function _GetTypes() {
            return [
                { key: null, text: i18n.t('label.all') },
                { key: clientTypeEnum.person, text: i18n.t('label.person') },
                { key: clientTypeEnum.company, text: i18n.t('label.company') }
            ];
        };
    };
})();