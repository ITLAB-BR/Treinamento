(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('logService', Service);

    Service.$inject = ['$http', '$q'];
    function Service($http, $q) {
        var listItensPrototype = [
            { Date: new Date(), Log: "[53] WARN  ITLabTemplate.Api.Controllers.CheckupController \nTeste de gravação de log!" },
            { Date: new Date(), Log: "[53] INFO  ITLabTemplate.Api.Controllers.CheckupController \nTeste de gravação de log!" },
            { Date: new Date(), Log: "[53] ERROR ITLabTemplate.Api.Controllers.CheckupController \nTeste de gravação de log!" }
        ];

        var service = {
            get: _Get
        };
        return service;

        function _Get(request) {
            var response = {
                data: listItensPrototype,
                status: 200
            };

            var deferred = $q.defer();
            deferred.resolve(response);
            return deferred.promise;
        };
    };
})();