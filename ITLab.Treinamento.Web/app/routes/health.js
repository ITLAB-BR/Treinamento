angular
    .module('itlabtreinamento')
    .config(['$navigationProvider', function (nav) {
        nav.map([
            {
                name: 'health',
                title: 'System Health',
                icon: 'medkit',
                nav: 'root',
                order: 5
            },
            {
                name: 'checkup',
                route: '^/checkup',
                title: 'Checkup',
                layout: 'external',
                template: 'app/pages/health/checkup.html',
                controller: 'CheckupController',
                bundle: [
                    'public/styles/pages/checkup.css',
                    'app/pages/health/checkup.service.js',
                    'app/pages/health/checkup.directive.js',
                    'app/pages/health/checkup.controller.js'
                ],
                anonymous: true,
                nav: 'health'
            },
            {
                name: 'log',
                route: '^/log',
                title: 'Log',
                layout: 'external',
                template: 'app/pages/health/logs.html',
                controller: 'logController',
                bundle: [
                    'app/pages/health/logs.controller.js',
                    //'app/pages/health/logs.service.js',
                    'app/pages/health/logs.service.prototype.js'
                ],
                anonymous: true,
                nav: 'health'
            }
        ]);
    }]);