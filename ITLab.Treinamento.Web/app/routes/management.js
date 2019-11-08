angular
    .module('itlabtreinamento')
    .config(['$navigationProvider', 'rolesEnum', function (nav, roles) {
        nav.map([
            {
                name: 'management',
                title: 'nav.manager',
                icon: 'briefcase',
                nav: 'root',
                order: 1
            },
            {
                name: 'countries',
                title: 'nav.countries',
                route: '/countries',
                template: 'app/pages/countries/crud.html',
                controller: 'countryController',
                bundle: [
                    //'app/pages/countries/crud.service.js',
                    'app/pages/countries/crud.service.prototype.js',
                    'app/pages/countries/crud.controller.js'
                ],
                roles: [roles.manageCountry],
                nav: 'management',
                order: 1
            },
            {
                name: 'clients-list',
                title: 'nav.clients',
                route: '/clients',
                template: 'app/pages/clients/list.html',
                controller: 'ClientsController',
                bundle: [
                    //'app/pages/clients/service.js',
                    'app/pages/clients/service.prototype.js',
                    'app/pages/clients/list.controller.js',
                    'app/core/services/form.service.js'
                ],
                nav: 'management',
                order: 2
            },
            {
                name: 'clients-edit',
                route: '/clients/:id',
                template: 'app/pages/clients/edit.html',
                controller: 'ClientController',
                bundle: [
                    //'app/pages/clients/service.js',
                    'app/pages/clients/service.prototype.js',
                    'app/pages/clients/edit.controller.js',
                    'app/core/services/form.service.js'
                ]
            },
            {
                name: 'orders-list',
                title: 'nav.orders',
                route: '/orders',
                template: 'app/pages/orders/list.html',
                controller: 'OrdersController',
                bundle: [
                    'app/pages/orders/service.prototype.js',
                    'app/pages/orders/list.controller.js',
                    'app/core/services/form.service.js'
                ],
                nav: 'management',
                order: 3
            },
            {
                name: 'orders-edit',
                route: '/orders/:id',
                template: 'app/pages/orders/edit.html',
                controller: 'OrderController',
                bundle: [
                    //'app/pages/orders/service.js',
                    'app/pages/orders/service.prototype.js',
                    'app/pages/orders/edit.controller.js',
                ]
            }
        ]);
    }]);