(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('checkupServicePrototype', Service);

    Service.$inject = ['$http', '$q'];
    function Service($http, $q) {

        var heartBeatPrototype = {
            baseAddressApi: 'http://localhost:8081/api/',
            languageDafault: 'pt-BR',
            languageCurrent: 'pt-BR',
            cacheFrontEndFiles: false,
            dateTimeNow: moment(),
            dateTimeNowUTC: moment.utc(),
            dateTimeUTCOffSet: moment().utcOffset() / 60,
            number: 12345.67
        };
        var dataBaseHeartBeatPrototype = true;
        var logInfoPrototype = [
            { LogFile: true, LogAppenderType: 'RollingFileAppender', FilePath: 'c:\\temp\\ITLabTemplateWebAPI\\log\\2016/12/05.txt' },
            { LogFile: false, LogAppenderType: 'ConsoleAppender', FilePath: undefined }
        ];
        var dataBaseInfoPrototype = {
            DataBaseName: 'ITLabTemplateWebAPI',
            ServerInstanceName: '.\\SQLExpress'
        };
        var smtpInfoPrototype = {
            DeliveryMethod: 'SMTP Server',
            Directory: '',
            DefaultFromAddress: 'template@itlab.com.br',
            Host: '',
            Port: '',
            EnableSsl: true,
            CredentialUsername: ''
        };
        var fileUploadInfoPrototype = {
            DirectoryTempUpload: 'c:\\temp\\ITLabTemplateWebAPI\\UploadFileTemp',
            FileCount: 2,
            FilesLengthInBytes: 5,
            MaxRequestLengthInBytes: 4
        };

        var testLogPrototype = true;
        var testSmtpPrototype = true;
        var testUploadDirectoryPrototype = true;

        var service = {
            getApiHeartBeat: _GetApiHeartBeat,
            getDataBaseHeartBeat: _GetDataBaseHeartBeat,
            getLogInfo: _GetLogInfo,
            getDataBaseInfo: _GetDataBaseInfo,
            getSmtpInfo: _GetSmtpInfo,
            getFileUploadInfo: _GetFileUploadInfo,

            testLog: _TestLog,
            testSmtp: _TestSmtp,
            testUploadDirectory: _TestUploadDirectory
        };
        return service;

        function _GetApiHeartBeat() {
            var response = {
                data: heartBeatPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _GetDataBaseHeartBeat() {
            var response = {
                data: dataBaseHeartBeatPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _GetLogInfo() {
            var response = {
                data: logInfoPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _GetDataBaseInfo() {
            var response = {
                data: dataBaseInfoPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _GetSmtpInfo() {
            var response = {
                data: smtpInfoPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _GetFileUploadInfo() {
            var response = {
                data: fileUploadInfoPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };

        //tests
        function _TestLog() {
            var response = {
                data: testLogPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _TestSmtp(email) {
            var response = {
                data: testSmtpPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
        function _TestUploadDirectory() {
            var response = {
                data: testUploadDirectoryPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
    };
})();