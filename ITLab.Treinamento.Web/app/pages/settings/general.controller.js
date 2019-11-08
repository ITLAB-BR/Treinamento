(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('GeneralSettingsController', controller);

    controller.$inject = ['$rootScope', 'generalSettingsService', 'commonsService'];
    function controller($rootScope, services, commonsService) {
        var viewModel = this;

        var SMTPDeliveryMethods = {
            SMTPServer: 0,
            LocalDirectory: 1
        };

        viewModel.itemEditing = {
            LayoutSkin: '',

            PasswordRequireDigit: false,
            PasswordRequireLowercase: false,
            PasswordRequireUppercase: false,
            PasswordRequireNonLetterOrDigit: false,
            PasswordRequiredMinimumLength: 3,
            PasswordHistoryLimit: 0,
            AccessTokenExpireTimeSpanInMinutes: 0,
            PasswordExpiresInDays: 0,

            UserLockoutEnabledByDefault: false,
            MaxFailedAccessAttemptsBeforeLockout: 0,
            DefaultAccountLockoutTimeInMinutes: 0,

            AuthenticateDataBase: false,
            AuthenticateActiveDirectory: false,

            ActiveDirectoryType: 1,
            ActiveDirectoryDomain: '',
            ActiveDirectoryDN: '',

            SMTPDefaultFromAddress: '',
            SMTPDeliveryMethod: 0,
            SMTPPickupDirectoryLocation: null,
            SMTPServerHost: null,
            SMTPServerPort: null,
            SMTPEnableSsl: null,
            SMTPCredentialsUsername: null,

            UploadDirectoryTemp: ''
        };

        viewModel.functions = {
            saveItem: saveItem,
            preview: setPreviewFunction()
        };

        viewModel.activeDirectoryTypes = services.activeDirectoryTypes;

        (function initializePage() {
            getData();
        })();

        function getData() {
            services.get(true).then(function (response) {
                if (response.status !== 200) {
                    commonsService.showMessage('alerts:error.get_generic', 'alerts:error.error');
                    return;
                }

                viewModel.itemEditing = response.data;
                if (viewModel.itemEditing.LayoutSkin === null)
                    viewModel.itemEditing.LayoutSkin = '';
            });
        }
        
        function saveItem() {
            if (!viewModel.itemEditing.AuthenticateActiveDirectory && !viewModel.itemEditing.AuthenticateDataBase) {
                commonsService.showMessage('alerts:warning.settings_authentication_type');
                return false;
            }

            if (viewModel.itemEditing.ActiveDirectoryType === services.activeDirectoryTypes.local) {
                viewModel.itemEditing.ActiveDirectoryDN = null;
                viewModel.itemEditing.ActiveDirectoryDomain = null;
            }

            var SMTPConfig = viewModel.itemEditing;
            if (!SMTPConfig.SMTPDefaultFromAddress || SMTPConfig.SMTPDefaultFromAddress.trim().length === 0 || SMTPConfig.SMTPDefaultFromAddress.indexOf('@') === -1 || SMTPConfig.SMTPDefaultFromAddress.indexOf('.') === -1 ) {
                commonsService.showMessage('alerts:error.smtp-default-from-address-invalid');
                return false;
            }
            if (Number(SMTPConfig.SMTPDeliveryMethod) === SMTPDeliveryMethods.LocalDirectory) {
                if (!SMTPConfig.SMTPPickupDirectoryLocation || SMTPConfig.SMTPPickupDirectoryLocation.trim().length === 0) {
                    commonsService.showMessage('alerts:error.smtp-pickup-directory-location');
                    return false;
                }
            } else if (Number(SMTPConfig.SMTPDeliveryMethod) === SMTPDeliveryMethods.SMTPServer) {
                if (!SMTPConfig.SMTPServerHost || SMTPConfig.SMTPServerHost.trim().length === 0) {
                    commonsService.showMessage('alerts:error.smtp-server-host');
                    return false;
                } else if (!SMTPConfig.SMTPServerPort || SMTPConfig.SMTPServerPort === 0) {
                    commonsService.showMessage('alerts:error.smtp-server-port');
                    return false;
                } else if (!SMTPConfig.SMTPCredentialsUsername || SMTPConfig.SMTPCredentialsUsername.trim().length === 0) {
                    commonsService.showMessage('alerts:error.smtp-credentials-username');
                    return false;
                } else if (!SMTPConfig.SMTPCredentialsPassword || SMTPConfig.SMTPCredentialsPassword.trim().length === 0) {
                    commonsService.showMessage('alerts:error.smtp-credentials-password');
                    return false;
                }
            }

            commonsService.setDisabledPropertyForAllButtonsFromScreen(true);
            var request = angular.copy(viewModel.itemEditing);
            if (request.LayoutSkin === '')
                request.LayoutSkin = null;
            services.putItem(request).then(function (response) {
                commonsService.setDisabledPropertyForAllButtonsFromScreen(false);

                if (response.status !== 200) {
                    commonsService.showMessage('alerts:error.save_generic', 'alerts:error.error');
                    return;
                }
                clearPreview();
                $rootScope.layoutSkin = response.data.LayoutSkin;

                toastr.success(i18n.t('alerts:success.save_generic'), i18n.t('alerts:success.success'));
            });
        }

        var clearPreview = function () { };
        function setPreviewFunction() {
            var skinPreview, rollBack;
            clearPreview = function () { clearTimeout(skinPreview); };

            return function (skin) {
                if (skinPreview) { rollBack(); clearPreview(); }

                $('body').removeClass($rootScope.layoutSkin).addClass(skin);

                rollBack = function () { var _skin = skin; $('body').addClass($rootScope.layoutSkin).removeClass(_skin); };
                skinPreview = setTimeout(rollBack, 5000);
            };
        }
    }
})();