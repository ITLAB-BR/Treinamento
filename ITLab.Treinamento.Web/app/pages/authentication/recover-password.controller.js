(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .controller('RecoverController', Controller);

    Controller.$inject = ['$state', 'forgotPasswordService', 'commonsService'];
    function Controller($state, itemService, commonsService) {
        var viewModel = this;

        viewModel.itemEditing = {
            email: ''
        };
        viewModel.functions = {
            sendRequestItem: _SendRequestItem
        };

        function _SendRequestItem() {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            itemService.resquestRecoveryPassword(viewModel.itemEditing.email)
                .then(function (response) {
                    commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                    if (response.status != 200) {
                        commonsService.showMessage('alerts:error.send_pass');
                        return;
                    }

                    commonsService.showMessage('alerts:success.send_pass');
                    $state.go('login');
                    return;
                });
        };
    };
})();