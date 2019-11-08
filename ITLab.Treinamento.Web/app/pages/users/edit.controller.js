(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('userEditController', controller);

    controller.$inject = ['$state', 'userService', 'commonsService'];
    function controller($state, services, commonsService) {
        var viewModel = this;

        viewModel.functions = {
            saveItem: saveItem
        };

        (function initializePage() {
            var itemId = angular.fromJson($state.params.id);
            getItem(itemId);

            getListGroups();
            getListCountries();
        })();

        function getItem(itemId) {
            services.getItem(itemId)
                .then(function (response) {
                    if (response.status != 200) {
                        commonsService.showMessage('alerts:error.get_user', 'alerts:error.error');
                        return;
                    }

                    viewModel.itemEditing = response.data || {};

                    viewModel._moment = {
                        LastPasswordChangedDate: moment(viewModel.itemEditing.LastPasswordChangedDate),
                        DateThatMustChangePassword: moment(viewModel.itemEditing.DateThatMustChangePassword)
                    };

                    if (viewModel.itemEditing.UserBlockedForManyAccess) {
                        viewModel._moment.LockoutEndDateUtc = moment(viewModel.itemEditing.LockoutEndDateUtc);

                        viewModel.updateLockoutTime = {
                            each: 1000,
                            stopAfterDate: viewModel.itemEditing.LockoutEndDateUtc
                        };
                    }

                    if (viewModel.itemEditing.AuthenticationTypeId == 1) {
                        viewModel.itemEditing.AuthenticationTypeDescription = i18n.t('label.db');
                    } else if (viewModel.itemEditing.AuthenticationTypeId == 2) {
                        viewModel.itemEditing.AuthenticationTypeDescription = i18n.t('label.ad');
                    }
                });
        }

        function saveItem() {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);

            services.putItem(viewModel.itemEditing)
                .then(function (response) {
                    commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                    if (response.status != 200) {
                        commonsService.showMessage('alerts:error.save_user', 'alerts:error.error');
                        return;
                    }

                    commonsService.showMessage('alerts:success.save_user', 'alerts:success.success');
                });
        }

        function getListGroups() {
            services.getListGroups()
                .then(function (response) {
                    if (response.status != 200) {
                        commonsService.showMessage('alerts:error.list_profile', 'alerts:error.error');
                        return;
                    }
                    viewModel.groupsList = response.data;
                });
        }

        function getListCountries() {
            services.getListCountries()
                .then(function (response) {
                    if (response.status != 200) {
                        commonsService.showMessage('alerts:error.list_client', 'alerts:error.error');
                        return
                    }
                    viewModel.countryList = response.data;
                });
        }

        if (services.prototypeMode) {
            (function workaroundCauseChangeAfterTime() {
                //Necess�rio somente em tempo de prot�tipo, ao ligar com as APIs esta fun��o pode ser exclu�da.
                //O motivo � o tempo (muito r�pido) do digest do angular, mais detalhes no artigo: https://stackoverflow.com/questions/15112584/how-do-i-use-scope-watch-and-scope-apply-in-angularjs
                setTimeout(function () {
                    viewModel.itemEditing.Countries = angular.copy(viewModel.itemEditing.Countries);
                    viewModel.itemEditing.Groups = angular.copy(viewModel.itemEditing.Groups);
                }, 1);
            })();
        }
    }
})();