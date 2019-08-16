angular
    .module('itlabtreinamento')
    .config(['$navigationProvider', 'rolesEnum', function (nav, roles) {
        nav.map([
            {
                name: 'settings',
                title: 'nav.settings',
                icon: 'cog',
                nav: 'root',
                order: 6
            },
            {
                name: 'general-settings',
                title: 'nav.general',
                route: '/settings/general',
                template: 'app/pages/settings/general.html',
                controller: 'GeneralSettingsController',
                bundle: [
                    //'app/pages/settings/general.service.js',
                    'app/pages/settings/general.service.prototype.js',
                    'app/pages/settings/general.controller.js'
                ],
                roles: [roles.manageGeneralSettings],
                nav: 'settings',
                order: 1
            },
            {
                name: 'api-clients',
                title: 'nav.api_clients',
                route: '/settings/api-clients',
                template: 'app/pages/settings/api-clients.html',
                controller: 'ApiClientController',
                bundle: [
                    //'app/pages/settings/api-clients.service.js',
                    'app/pages/settings/api-clients.service.prototype.js',
                    'app/pages/settings/api-clients.controller.js'
                ],
                nav: 'settings',
                order: 2
            }
        ]);
    }]);