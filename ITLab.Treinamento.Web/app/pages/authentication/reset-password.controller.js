(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('ResetController', Controller);

    Controller.$inject = ['$state', 'resetPasswordService', 'commonsService'];
    function Controller($state, itemService, commonsService) {
        var userId = $state.params.userId;

        var viewModel = this;

        viewModel.itemEditing = {
            email: '',
            newPassword: '',
            confirmPassword: ''
        };
        viewModel.listItems = [];
        viewModel.functions = {
            saveItem: _SaveItem
        };



        function _SaveItem() {
            if (viewModel.passwordNew !== viewModel.passwordConfirm) {
                var msg = i18n.t('alerts:error.compare_pass');
                toastr.warning(msg);
                return;
            }
            var request = {
                password: viewModel.passwordNew,
                confirmPassword: viewModel.passwordConfirm,
                email: viewModel.email,
                code: $state.params.code
            };

            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            itemService.resetPassword(request).then(function (response) {
                commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                if (response.status != 200) {
                    commonsService.showMessages(response.data.Errors, 'alerts:error.error');
                    return;
                }

                commonsService.showMessage('alerts:success.change_pass');
                $state.go('login');
            });
        };

        function _GetUserById() {
            itemService.getUser(userId).then(function (res) {
                if (res.status != 200) {
                    commonsService.showMessage('alerts:error.get_user');
                    return;
                }

                var user = res.data || {};
                viewModel.email = user.Email;
            });
        };
    }
})();