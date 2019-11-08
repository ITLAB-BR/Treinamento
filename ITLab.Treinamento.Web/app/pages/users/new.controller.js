(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('userNewController', controller);

    controller.$inject = ['$state', 'userService', 'commonsService', 'generalSettingsService'];
    function controller($state, services, commonsService, generalSettingsService) {
        var viewModel = this;
        
        viewModel.itemEditing = {
            AuthenticationTypeId: 0
        };

        viewModel.authenticationTypesAvailable = {
            dataBase: {
                id: services.authenticationTypes.dataBase,
                active: false
            },
            activeDirectory: {
                id: services.authenticationTypes.activeDirectory,
                active: false
            }
        };

        viewModel.functions = {
            saveItem: saveItem
        };

        (function initializePage() {
            getGeneralSettings();
        })();

        function getGeneralSettings() {
            generalSettingsService.get().then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage(response.data);
                    return;
                }

                viewModel.authenticationTypesAvailable.activeDirectory.active = response.data.AuthenticateActiveDirectory;
                viewModel.authenticationTypesAvailable.dataBase.active = response.data.AuthenticateDataBase;

                if (viewModel.authenticationTypesAvailable.dataBase.active) {
                    viewModel.itemEditing.AuthenticationTypeId = viewModel.authenticationTypesAvailable.dataBase.id;
                }
                else if (viewModel.authenticationTypesAvailable.activeDirectory.active) {
                    viewModel.itemEditing.AuthenticationTypeId = viewModel.authenticationTypesAvailable.activeDirectory.id;
                }
            });
        }

        function saveItem() {
            if (viewModel.itemEditing.Password1 !== viewModel.itemEditing.Password2) {
                commonsService.showMessage('alerts:error.compare_pass', 'alerts:error.error');
                return;
            }
            
            viewModel.itemEditing.Password = viewModel.itemEditing.Password1;

            var request = viewModel.itemEditing;

            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            services.postItem(request).then(function (response) {
                commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                if (response.status != 200) {
                    commonsService.showMessages(response.data.Errors, 'alerts:error.error');
                    return;
                }

                var params = { id: response.data };
                $state.go('users-edit', params);

                commonsService.showMessages('alerts:success.create_user', 'alerts:success.success');
            });
        }
    }

})();