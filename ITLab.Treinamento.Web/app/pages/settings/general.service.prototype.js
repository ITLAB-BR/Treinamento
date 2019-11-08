(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('generalSettingsService', service);

    service.$inject = ['$q'];
    function service($q) {
        var activeDirectoryTypes = {
            local: 1,
            server: 2
        };

        var dataBasePrototype = {
            generalSettings: {
                LayoutSkin: 'skin-grey',

                PasswordRequireDigit: true,
                PasswordRequireLowercase: false,
                PasswordRequireUppercase: true,
                PasswordRequireNonLetterOrDigit: false,
                PasswordRequiredMinimumLength: 3,
                PasswordHistoryLimit: 3,
                AccessTokenExpireTimeSpanInMinutes: 60 * 24,
                PasswordExpiresInDays: 7,

                UserLockoutEnabledByDefault: false,
                MaxFailedAccessAttemptsBeforeLockout: 0,
                DefaultAccountLockoutTimeInMinutes: 0,

                AuthenticateDataBase: true,
                AuthenticateActiveDirectory: true,

                ActiveDirectoryType: activeDirectoryTypes.local,
                ActiveDirectoryDomain: null,
                ActiveDirectoryDN: null,

                SMTPDefaultFromAddress: 'template@itlab.com.br',
                SMTPDeliveryMethod: 1,
                SMTPPickupDirectoryLocation: 'c:\\temp\\email',
                SMTPServerHost: null,
                SMTPServerPort: null,
                SMTPEnableSsl: null,
                SMTPCredentialsUsername: null,

                UploadDirectoryTemp: 'c:\\temp'
            }
        };

        var service = {
            activeDirectoryTypes: activeDirectoryTypes,

            get: get,
            putItem: putItem
        };
        return service;

        function get(refresh, setting) {
            var deferred = $q.defer();

            var result = dataBasePrototype.generalSettings;

            if (setting) {
                angular.forEach(result, function (value, key) {
                    if (setting == key) {
                        result = value;
                        return;
                    }
                });
            }

            var response = {
                data: result,
                status: 200
            };

            deferred.resolve(response);
            return deferred.promise;
        }

        function putItem(request) {

            dataBasePrototype.generalSettings = angular.copy(request);

            var response = {
                data: dataBasePrototype.generalSettings,
                status: 200
            };
            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        }
    }

})();