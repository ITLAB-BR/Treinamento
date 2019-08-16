(function () {
    'use strict';

    angular.module('itlabtreinamento').factory('changePasswordService', Service);

    Service.$inject = ['$http'];

    function Service($http) {
        var service = {
            change: change,
            changePasswordExpired: changePasswordExpired
        };
        
        return service;
    
        function change(request) {
            return $http({
                url: '/api/Password/ChangeMyPasswordAsync',
                method: 'POST',
                data: request
            });
        };

        function changePasswordExpired(request) {
            return $http({
                url: '/api/Password/ChangeMyPasswordExpiredAsync',
                method: 'POST',
                data: request
            });
        };

    };
})();
