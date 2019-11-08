angular
    .module('itlabtreinamento')
    .config(['$navigationProvider', 'rolesEnum', function (nav, roles) {
        nav.map([
            {
                name: 'login',
                route: '/login',
                layout: 'account',
                template: 'app/pages/authentication/login.html',
                controller: 'LoginController',
                bundle: [
                    'app/pages/authentication/change-password.service.js',
                    'app/pages/authentication/login.controller.js'
                ],
                anonymous: true
            },
            {
                name: 'recover-password',
                route: '/recover-password',
                template: 'app/pages/authentication/recover-password.html',
                controller: 'RecoverController',
                bundle: [
                    'app/pages/authentication/recover-password.service.js',
                    'app/pages/authentication/recover-password.controller.js'
                ],
                anonymous: true
            },
            {
                name: 'reset-password',
                route: '/reset-password?userId&code',
                template: 'app/pages/authentication/reset-password.html',
                controller: 'ResetController',
                bundle: [
                    'app/pages/authentication/reset-password.service.js',
                    'app/pages/authentication/reset-password.controller.js'
                ],
                anonymous: true
            },
            {
                name: 'change-password',
                route: '/account/change-password',
                template: 'app/pages/authentication/change-password.html',
                controller: 'ChangePasswordController',
                bundle: [
                    'app/pages/authentication/change-password.service.js',
                    'app/pages/authentication/change-password.controller.js'
                ],
                roles: [roles.userChangePassword]
            },
            {
                name: 'my-account',
                route: '/account',
                template: 'app/pages/account/edit.html',
                controller: 'accountEditController',
                bundle: [
                    //'app/pages/account/edit.service.js',
                    'app/pages/account/edit.service.prototype.js',
                    'app/pages/account/edit.controller.js'
                ],
                roles: [roles.manageUser]
            },
            {
                name: 'security',
                title: 'nav.security',
                icon: 'lock',
                nav: 'root',
                roles: [roles.manageUser],
                order: 4
            },
            {
                name: 'users-list',
                title: 'nav.users',
                route: '/users',
                template: 'app/pages/users/list.html',
                controller: 'userListController',
                bundle: [
                    //'app/pages/countries/crud.service.js',
                    //'app/pages/users/service.js',
                    'app/pages/countries/crud.service.prototype.js',
                    'app/pages/users/service.prototype.js',
                    'app/pages/users/list.controller.js'
                ],
                roles: [roles.manageUser],
                nav: 'security',
                order: 1
            },
            {
                name: 'users-new',
                title: 'nav.new_user',
                route: '/users/new',
                template: 'app/pages/users/new.html',
                controller: 'userNewController',
                bundle: [
                    //'app/pages/countries/crud.service.js',
                    //'app/pages/settings/general.service.js',
                    //'app/pages/users/service.js',
                    'app/pages/countries/crud.service.prototype.js',
                    'app/pages/settings/general.service.prototype.js',
                    'app/pages/users/service.prototype.js',
                    'app/pages/users/new.controller.js'
                ],
                roles: [roles.manageUser],
                nav: 'security',
                order: 2
            },
            {
                name: 'users-edit',
                route: '/users/:id',
                template: 'app/pages/users/edit.html',
                controller: 'userEditController',
                bundle: [
                    //'app/pages/countries/crud.service.js',
                    //'app/pages/users/service.js',
                    'app/pages/countries/crud.service.prototype.js',
                    'app/pages/users/service.prototype.js',
                    'app/pages/users/edit.controller.js'
                ],
                roles: [roles.manageUser]
            },
            {
                name: 'access-groups',
                title: 'nav.groups',
                route: '/security/access-groups',
                template: 'app/pages/authorization/groups.html',
                controller: 'groupsController',
                bundle: [
                    //'app/pages/countries/crud.service.js',
                    //'app/pages/users/service.js',
                    //'app/pages/authorization/groups.service.js',
                    'app/pages/countries/crud.service.prototype.js',
                    'app/pages/users/service.prototype.js',
                    'app/pages/authorization/groups.service.prototype.js',
                    'app/pages/authorization/groups.controller.js'
                ],
                roles: [roles.managerProfile],
                nav: 'security',
                order: 3
            }
        ]);
    }]);