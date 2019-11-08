(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('checkupService', Service);

    Service.$inject = ['$http'];
    function Service($http) {
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
            return $http({
                url: '/api/Checkup/GetApiHeartBeat',
                method: 'GET'
            });
        };
        function _GetDataBaseHeartBeat() {
            return $http({
                url: '/api/Checkup/GetDataBaseHeartBeat',
                method: 'GET'
            });
        };
        function _GetLogInfo() {
            return $http({
                url: '/api/Checkup/GetLogInfo',
                method: 'GET'
            });
        };
        function _GetDataBaseInfo() {
            return $http({
                url: '/api/Checkup/GetDataBaseInfo',
                method: 'GET'
            });
        };
        function _GetSmtpInfo() {
            return $http({
                url: '/api/Checkup/GetSMTPInfo',
                method: 'GET'
            });
        };
        function _GetFileUploadInfo() {
            return $http({
                url: '/api/Checkup/GetFileUploadInfo',
                method: 'GET'
            });
        };

        //tests
        function _TestLog() {
            return $http({
                url: '/api/Checkup/TestLogDirectory',
                method: 'GET'
            });
        };
        function _TestSmtp(email) {
            var request = { destinationAddress: email }
            return $http({
                url: '/api/Checkup/TestSMTP',
                method: 'GET',
                params: request
            });
        };
        function _TestUploadDirectory() {
            return $http({
                url: '/api/Checkup/TestUploadDirectory',
                method: 'GET'
            });
        };
    };
})();