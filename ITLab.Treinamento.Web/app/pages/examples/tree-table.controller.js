(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .controller('TreeTableController', Controller);

    Controller.$inject = ['TreeTableService'];
    function Controller(itemService) {
        var viewModel = this;

        viewModel.listItems = [];
        var columns = [
            { param: 'id', label: 'id' },
            { param: 'name', label: 'Nome' },
            { param: 'col1', label: 'Coluna 1' },
            { param: 'col2', label: 'Coluna 2' },
            { param: 'col3', label: 'Coluna 3' },
            { param: 'col4', label: 'Coluna 4' },
            { param: 'col5', label: 'Coluna 5' },
        ];

        var groups = [columns[5], columns[2]];
        var panelHeader = [
            { fn: _minCol3, description: 'Mínimo da Coluna 3', class: 'text-primary' },
            { fn: _somaCol5, description: 'Soma da Coluna 5', class: 'm-l', style: 'width:160px' },
        ];

        viewModel.config = {
            columns: columns,
            groups: groups,
            panelHeader: panelHeader,
            loading: _loading
        };

        function _minCol3(sublist) {
            var aux = Infinity;
            for (var i = 0; i < sublist.length; i++) {
                if (aux > sublist[i].col3)
                    aux = sublist[i].col3;
            }
            return aux;
        }
        function _somaCol5(sublist) {
            var aux = 0;
            for (var i = 0; i < sublist.length; i++) {
                aux += sublist[i].col5;
            }
            return aux;
        }

        (function initializePage() {
            _ListItems();
        })();

        //
        function _ListItems() {
            viewModel.listItems = [];
            return itemService.list().then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.message);
                    return;
                }

                var data = response.data;
                viewModel.listItems = angular.copy(data);

                return response;
            });
        }

        function _loading(isLoading) {
            if (isLoading)
                console.log('start rendering');
            else
                console.log('finish rendering');
        }
    }

})();