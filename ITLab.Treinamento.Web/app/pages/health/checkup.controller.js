(function () {
    'use strict';

    angular.module('itlabtreinamento').controller('CheckupController', Controller);

    Controller.$inject = ['$scope', 'checkupService', 'commonsService', 'generalConfig', 'multilanguage'];
    function Controller($scope, itemService, commonsService, generalConfig, multilanguage) {
        var viewModel = this;
        
        viewModel.itemEditing = {
            frontEnd: {
                templateVersion: generalConfig.templateVersion,
                baseAddressApi: generalConfig.baseAddressApi,
                languageDafault: generalConfig.language.languageDefault,
                languageCurrent: multilanguage.currentLanguage,
                cacheFrontEndFiles: generalConfig.cacheFrontEndFiles,
                dateTimeNow: moment(),
                dateTimeNowUTC: moment.utc(),
                dateTimeUTCOffSet: moment().utcOffset() / 60,
                number: 12345.67
            },
            testApiHeartBeatResult: false,
            log: [],
            testLogResult: false,
            database: {
                DataBaseName: '',
                ServerInstanceName: ''
            },
            testDataBaseHeartBeatResult: false,
            smtp: {
                DeliveryMethod: '',
                Directory: '',
                DefaultFromAddress: '',
                Host: '',
                Port: '',
                EnableSsl: false,
                CredentialUsername: ''
            },
            sentMail: false,
            fileUpload: {
                DirectoryTempUpload: '',
                FileCount: undefined,
                FilesLengthInBytes: undefined,
                MaxRequestLengthInBytes: 0
            },
            testUploadResult: false
        };
        viewModel.functions = {
            testSmtp: _TestSmtp
        };

        var enumStateButton = {
            //TODO: traduzir
            'static': { text: 'Testar' },
            loading: { text: 'Enviando...' },
            success: { text: 'OK!', color: 'primary' },
            fail: { text: 'Fail! Try again', color: 'danger' }
        };
        var logButton = enumStateButton.static;

        (function initializePage() {
            _GetInfos();
            _TestLog();
        })();

        function _GetInfos() {
            itemService.getApiHeartBeat().then(function (response) {
                if (response.status != 200) {
                    viewModel.itemEditing.testApiHeartBeatResult = false;
                    return;
                }

                viewModel.itemEditing.testApiHeartBeatResult = response.data;
            });

            itemService.getDataBaseHeartBeat().then(function (response) {
                if (response.status != 200) {
                    viewModel.itemEditing.testDataBaseHeartBeatResult = false;
                    return;
                }

                viewModel.itemEditing.testDataBaseHeartBeatResult = response.data;
            });

            itemService.getLogInfo().then(function (response) {
                if (response.status != 200) {
                    viewModel.itemEditing.testLogResult = false;
                    return;
                }

                viewModel.itemEditing.log = response.data;
                _TestLog();
            });

            itemService.getDataBaseInfo().then(function (response) {
                if (response.status != 200)
                    return;

                viewModel.itemEditing.database = response.data;
            });

            itemService.getSmtpInfo().then(function (response) {
                if (response.status != 200) {
                    viewModel.sentMail = false;
                    return;
                }
                viewModel.itemEditing.smtp = response.data;

                if (viewModel.itemEditing.smtp.DeliveryMethod == 'Directory') {
                    _TestSmtp('teste@teste');
                }
            });

            itemService.getFileUploadInfo().then(function (response) {
                if (response.status != 200) {
                    viewModel.testUploadResult = false;
                    return;
                }

                viewModel.itemEditing.fileUpload = response.data;
                _TestUpload();
            });
        };
        function _TestLog() {
            viewModel.testLogLoading = true;
            viewModel.testLogResult = undefined;
            logButton = enumStateButton.loading;

            itemService.testLog().then(function (response) {
                viewModel.itemEditing.logButton = enumStateButton.static;
                viewModel.testLogLoading = false;

                if (response.status != 200)
                    return;

                viewModel.itemEditing.testLogResult = response.data;
            })
        };
        function _TestSmtp(email) {
            if (!email) {
                commonsService.showMessage('alerts:warning.invalid_email');
                return;
            }
            $scope.testSmtpLoading = true;

            itemService.testSmtp(email)
                .then(function (response) {
                    $scope.testSmtpLoading = false;
                    if (response.status != 200) {
                        viewModel.sentMail = false;
                    } else {
                        viewModel.itemEditing.sentMail = response.data;
                    }
                });
        };
        function _TestUpload() {
            viewModel.testUploadResult = undefined;

            itemService.testUploadDirectory()
                .then(function (response) {
                    if (response.status != 200) {
                        viewModel.testUploadResult = false;
                    } else {
                        viewModel.itemEditing.testUploadResult = response.data;
                    }
                })
        };
    };
})();