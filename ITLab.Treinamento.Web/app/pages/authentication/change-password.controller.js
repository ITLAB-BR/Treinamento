(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .controller('ChangePasswordController', Controller);

    Controller.$inject = ['$state', 'changePasswordService', 'commonsService', 'generalConfig'];
    function Controller($state, itemService, commonsService, generalConfig) {
        var viewModel = this;

        viewModel.itemEditing = {
            passwordOld: '',
            passwordNew: '',
            passwordConfirm: ''
        };
        viewModel.functions = {
            saveItem: _SaveItem
        };

        function _SaveItem() {
            if (viewModel.itemEditing.passwordNew !== viewModel.itemEditing.passwordConfirm) {
                commonsService.showMessage('alerts:error.compare_pass');
                return;
            }
            var request = {
                oldPassword: viewModel.itemEditing.passwordOld,
                newPassword: viewModel.itemEditing.passwordNew,
                confirmPassword: viewModel.itemEditing.passwordConfirm
            };

            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            itemService.change(request).then(function (reponse) {
                commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                if (reponse.status !== 200) {
                    if (reponse.data && reponse.data.Errors)
                        commonsService.showMessages(reponse.data.Errors, 'alerts:error.error');
                    return;
                }
                commonsService.showMessage('alerts:success.change_pass', 'alerts:success.success');

                $state.go(generalConfig.defaultPage);
            });
        };
    }
})();