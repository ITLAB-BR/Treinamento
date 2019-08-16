(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .controller('accountEditController', controller);

    controller.$inject = ['$state', 'accountEditService', 'commonsService', 'getModelAsFormData'];
    function controller($state, itemService, commonsService, getModelAsFormData) {
        var viewModel = this;

        viewModel.itemEditing = {
            Login: '',
            Email: '',
            Name: '',
            Photo: null,
            image: null,
            filesToUpload: [],
            removePhoto: false
        };

        viewModel.functions = {
            saveItem: saveItem
        };

        (function initializePage() {
            getUserForEdit();
            getUserAvatar();
        })()

        function getUserForEdit() {
            itemService.get().then(function (response) {
                viewModel.itemEditing = response.data;
            });
        }

        function getUserAvatar() {
            commonsService.getUserAvatar().then(function (response) {
                if (response.status == 200) {
                    viewModel.itemEditing.Photo = response.data;
                }
            });
        }

        function saveItem() {
            var request = getModelAsFormData(viewModel.itemEditing, 'image');

            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            itemService.set(request)
                .then(function (response) {
                    commonsService.setDisabledPropertyForAllButtonsFromScreen(false);

                    viewModel.itemEditing.image = null;

                    if (response.status != 200) {
                        commonsService.showMessage('alerts:error.save_generic', 'alerts:error.error');
                        return;
                    }
                    commonsService.showMessage('alerts:success.save_generic', 'alerts:success.success');
                    if (viewModel.itemEditing.removePhoto) {
                        document.getElementById('fileUpload').value = '';
                    }

                    $state.go('home');
                })
        }
    }
})()