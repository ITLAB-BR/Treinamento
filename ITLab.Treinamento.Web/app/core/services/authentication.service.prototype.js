(function () {
    'use strict';

    angular.module('itlabtreinamento')
        .factory('principal', principalService)
        .factory('authIdentity', authIdentityService)
        .factory('authToken', authTokenService)
        .factory('authRole', authRoleService)
        .factory("signoutService", signout);

    principalService.$inject = ['$q', 'authIdentity', 'authRole'];
    function principalService($q, authIdentity, authRole) {
        var _identity;
        var authenticated = false;

        var service = {
            isIdentityResolved: isIdentityResolved,
            isAuthenticated: isAuthenticated,
            isInRole: isInRole,
            isInAnyRole: isInAnyRole,
            authenticate: authenticate,
            identity: identity,
            getUserNameLogged: getUserNameLogged,
            dispose: dispose
        };
        return service;


        function isIdentityResolved() {
            return angular.isDefined(_identity);
        }

        function isAuthenticated() {
            return authenticated && !authIdentity.isExpired();
        }

        function isInRole(role) {
            if (!authenticated || !_identity.roles) return false;

            return _identity.roles.indexOf(role) != -1;
        }

        function isInAnyRole(roles) {
            if (!authenticated || !_identity.roles) return false;

            for (var i = 0; i < roles.length; i++) {
                if (this.isInRole(roles[i])) return true;
            }

            return false;
        }

        function authenticate(identity) {
            var deferred = $q.defer();

            _identity = identity;
            authenticated = identity != null;

            if (identity)
                authIdentity.set(identity);

            if (!identity || authIdentity.isExpired()) {
                authIdentity.remove();
                _identity = undefined;
                deferred.resolve();
            } else {
                authRole.get(_identity.userName)
                        .then(function (response) {
                            identity.roles = response.data;
                            _identity = identity;

                            authIdentity.set(identity);
                            deferred.resolve();
                        });
            }

            return deferred.promise;
        }

        function identity(force) {
            var deferred = $q.defer();

            if (force === true) _identity = undefined;

            if (isIdentityResolved()) {
                deferred.resolve(_identity);

                return deferred.promise;
            }

            _identity = authIdentity.get();
            return authenticate(_identity);
        }

        function getUserNameLogged() {
            var identity = _identity || authIdentity.get();

            return !!identity ? identity.userName : '';
        }

        function dispose() {
            authIdentity.remove();
            _identity = undefined;
        }
    }

    function authIdentityService() {
        var service = {
            get: get,
            set: set,
            remove: remove,
            isExpired: isExpired
        };
        return service;

        function get() {
            return angular.fromJson(localStorage.getItem('user.identity'));
        }

        function set(identity) {
            localStorage.setItem('user.identity', angular.toJson(identity));
        }

        function remove() {
            localStorage.removeItem('user.identity');
        }

        function isExpired() {
            var identity = get();

            if (!identity) return true;

            var expiresIn = (new Date(identity['.expires'])).getTime();
            var nowSeconds = (new Date()).getTime();

            return expiresIn < nowSeconds;
        }
    }

    authTokenService.$inject = ['$q', 'generalConfig'];
    function authTokenService($q, generalConfig) {
        var itemPrototype = {
            access_token: 'token_fake',
            token_type: 'bearer',
            expires_in: 1799,
            userName: 'admin',
            'as:client_id': 'WebAngularAppAuth',
            '.issued': new Date(),
            '.expires': moment().add(30, 'minutes').toDate()
        };

        var service = {
            get: get
        };
        return service;

        function get(username, password) {
            var deferred = $q.defer();

            var response = {
                data: itemPrototype,
                status: 200
            };

            if (!(username == 'admin' && password == '123456')) {
                response = {
                    data: {
                        error: 'invalid_grant',
                        error_description: 'The user name or password is incorrect.'
                    },
                    status: 400
                };
            }

            deferred.resolve(response);
            return deferred.promise;
        }
    }

    authRoleService.$inject = ['$q'];
    function authRoleService($q) {
        var service = {
            get: get
        };
        return service;

        function get(userName) {
            var deferred = $q.defer();

            var response = {
                data: '[-3,-2,-1,0,1,2,3,4,5,6]',
                status: 200
            };

            deferred.resolve(response);

            return deferred.promise;
        }
    }

    signout.$inject = ['principal', 'topNotificationService'];
    function signout(principal, topNotificationService) {
        return function () {
            principal.dispose();
            topNotificationService.stopRefreshNotification();
        }
    }

})();