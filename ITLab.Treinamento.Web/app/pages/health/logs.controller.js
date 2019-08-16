(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('logController', controller);

    controller.$inject = ['logService', 'commonsService', 'DaterangepickerOptions'];
    function controller(itemService, commonsService, DaterangepickerOptions) {
        var viewModel = this;

        viewModel.filter = {
            date: moment()
        };
        viewModel.listItems = [];
        viewModel.functions = {
            searchFilter: _SearchFilter
        };
        viewModel.dateOptions = DaterangepickerOptions.singleConfig;

        (function initializePage() {
            _SearchFilter();
        })();

        function _SearchFilter() {
            var request = { date: viewModel.filter.date.format() };

            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            itemService.get(request).then(function (response) {
                commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                if (response.status !== 200) {
                    commonsService.showMessages(response.data.message);
                    viewModel.listItems = null;
                    return;
                }

                viewModel.listItems = response.data;
            });
        };
    };
})();