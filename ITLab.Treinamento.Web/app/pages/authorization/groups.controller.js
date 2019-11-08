(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('groupsController', controller);

    controller.$inject = ['groupsService', 'commonsService', 'userService'];
    function controller(services, commonsService, userService) {
        var viewModel = this;

        var itemDefault = {
            action: 'new',
            index: undefined,
            Id: 0,
            Name: '',
            Active: true
        }

        viewModel.listItems = [];
        viewModel.listRoles = [];
        viewModel.listUsers = [];

        viewModel.itemEditing = null;

        viewModel.functions = {
            newItem: newItem,
            selectItem: selectItem,
            saveItem: saveItem,

            setRolesToGroup: setRolesToGroup,
            setPermissionToUsers: setPermissionToUsers
        };

        (function initializePage() {
            getListItems();
            getUsers();
            getRoles();
        })();

        function newItem() {
            viewModel.itemEditing = angular.copy(itemDefault);
            viewModel.itemEditing.action = 'new';
        }

        function selectItem(item) {
            viewModel.itemEditing = angular.copy(item);
            viewModel.itemEditing.index = viewModel.listItems.indexOf(item);
            viewModel.itemEditing.action = 'edit';
        }

        function saveItem() {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            var request = angular.copy(viewModel.itemEditing);

            if (viewModel.itemEditing.action == 'new') {
                services.postItem(request).then(function (response) {
                    viewModel.listItems.push(response.data);
                    finalizeSave(response);
                });
            } else if (viewModel.itemEditing.action == 'edit') {
                services.putItem(request).then(function (response) {
                    viewModel.listItems[viewModel.itemEditing.index] = viewModel.itemEditing;
                    finalizeSave(response);
                });
            }
        }

        function finalizeSave(response) {
            commonsService.setDisabledPropertyForAllButtonsFromScreen(false);

            if (response.status != 200) {
                commonsService.showMessage(response.data.message);
                return;
            }

            commonsService.showMessage('alerts:success.save_generic');
            $('.modal').modal('hide');
        }

        function setRolesToGroup() {
            var request = { GroupId: viewModel.itemEditing.Id, RolesIds: viewModel.itemEditing.Roles };
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            services.setRolesToGroup(request).then(function (response) {
                commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                if (response.status != 200) {
                    commonsService.showMessage('alerts:error.save_profile_permissions', 'alerts:error.error');
                    return;
                }
                viewModel.listItems[viewModel.itemEditing.index] = viewModel.itemEditing;
                commonsService.showMessage('alerts:success.save_profile_permissions', 'alerts:success.success');
                $('.modal').modal('hide');
            });
            return false;
        }

        function setPermissionToUsers() {
            var request = { GroupId: viewModel.itemEditing.Id, UsersIds: viewModel.itemEditing.Users };
            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            services.setPermissionToUsers(request).then(function (response) {
                commonsService.setDisabledPropertyForAllButtonsFromScreen(false);
                if (response.status != 200) {
                    commonsService.showMessage('alerts:error.save_profile_users', 'alerts:error.error');
                    return;
                }

                viewModel.listItems[viewModel.itemEditing.index] = viewModel.itemEditing;
                commonsService.showMessage('alerts:success.save_profile_users', 'alerts:success.success');
                $('.modal').modal('hide');
            });
        }

        function getListItems() {
            services.getList().then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage('alerts:error.unexpected', 'alerts:error.error');
                    return;
                }
                viewModel.listItems = response.data;
            });
        }

        function getUsers() {
            userService.getList().then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage('alerts:error.unexpected', 'alerts:error.error');
                    return;
                }
                viewModel.listUsers = response.data;
            });
        }

        function getRoles() {
            services.getRoles().then(function (response) {
                if (response.status != 200) {
                    commonsService.showMessage('alerts:error.unexpected', 'alerts:error.error');
                    return;
                }
                viewModel.listRoles = response.data;
            });
        }
    }

})();