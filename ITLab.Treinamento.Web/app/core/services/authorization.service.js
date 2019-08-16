(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .factory('authorization', service);

    service.$inject = ['$q', '$rootScope', '$state', 'principal'];
    function service($q, $rootScope, $state, principal) {
        var service = {
            authorize: authorize
        };
        return service;


        function authorize() {
            var deferred = $q.defer();

            principal.identity()
                     .then(function () {
                         var isAuthenticated = principal.isAuthenticated();

                         if (!isAuthenticated) {
                             deferred.reject();
                             principal.dispose();
                             $state.go('login');
                         }

                         if ($rootScope.toState.data.roles &&
                             $rootScope.toState.data.roles.length > 0 &&
                             !principal.isInAnyRole($rootScope.toState.data.roles)) {
                             deferred.reject();
                             $state.go('accessdenied');
                         }

                         deferred.resolve();
                     });

            return deferred.promise;
        };
    }

}());