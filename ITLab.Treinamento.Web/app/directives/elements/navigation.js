angular
    .module('itlabtreinamento')
    .directive('navigation', function () {
        return {
            restrict: 'E',
            scope: {},
            templateUrl: '/app/directives/elements/navigation.html',
            controller: ['$scope', 'commonsService', 'signoutService', 'principal', '$navigation', function ($scope, commonsService, signoutService, principal, $navigation) {
                $scope.menu = $navigation.getTree();
                $scope.signout = signoutService;

                $scope.userHasPermissionInRoles = function (rolesToVerify) {
                    if (!rolesToVerify || rolesToVerify.length == 0) {
                        return true;
                    }
                    if (!principal.isAuthenticated()) {
                        return false;
                    }

                    return principal.isInAnyRole(rolesToVerify);
                };

                commonsService.getUserAvatar().then(function (res) {
                    if (res.status == 200) {
                        $scope.loggedUserAvatar = res.data;
                    }
                });
            }]
        };
    });