(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('LoginController', controller);
    controller.$inject = ['$scope', '$state', '$rootScope', 'authToken', 'principal', 'changePasswordService', 'commonsService', 'generalConfig', 'spinnerFullPage'];
    function controller($scope, $state, $rootScope, authToken, principal, itemService, commonsService, generalConfig, spinnerFullPage) {
        var viewModel = this;

        viewModel.itemEditing = {
            username: '',
            password: '',
            newPassword: '',
            confirmPassword: ''
        };
        viewModel.functions = {
            authenticate: authenticate,
            changePassword: changePassword
        };

        viewModel.changePasswordExpired = false;

        (function initializePage() {

        })();

        function isAuthenticated(response) {
            if (response.status === 200 && !(response.data && response.data.error)) return true;

            var error = response.data && response.data.error || '';

            var msgTitleError = 'alerts:error.error';
            var msgTitleDeniedAccess = 'alerts:error.denied_access';

            if (/invalid_grant|user_disabled|user_locked|user_requiresVerification/.test(error)) {
                commonsService.showMessage(error, msgTitleDeniedAccess);
            } else if (error.indexOf('user_not_found') != -1) {
                commonsService.showMessage(error, msgTitleError);
            } else if (error.indexOf('user_password_expired') != -1) {
                commonsService.showMessage(error, msgTitleDeniedAccess);
                viewModel.changePasswordExpired = true;
            } else {
                commonsService.showMessage('alerts:error.unknown', msgTitleError);
            }

            return false;
        }

        function authenticate() {
            spinnerFullPage.show(true);
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            authToken.get(viewModel.itemEditing.username, viewModel.itemEditing.password)
                     .then(function (response) {
                         spinnerFullPage.show(false);
                         commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                         if (isAuthenticated(response)) {
                             principal.authenticate(response.data)
                                      .then(function () {
                                          $rootScope.loggedUser = principal.getUserNameLogged();

                                          $state.go(generalConfig.defaultPage);
                                      });
                         }
                     });
        }

        function changePassword() {
            //TODO: Esta função precisa ser refatorada, principalmente referente as mensagens de error 
            var request = {
                username: viewModel.itemEditing.username,
                oldPassword: viewModel.itemEditing.password,
                newPassword: viewModel.itemEditing.newPassword,
                confirmPassword: viewModel.itemEditing.confirmPassword
            };
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            itemService.changePasswordExpired(request)
                            .then(function (response) {
                                commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                                if (response.status !== 200) {
                                    commonsService.showMessages(response.data.Errors);
                                    return;
                                }
                                commonsService.showMessage('alerts:success.change_pass');
                                viewModel.changePasswordExpired = false;
                            });
        };
    }
})();